using System;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Realtorist.DataAccess.Abstractions;
using Realtorist.Models.Settings;
using Realtorist.Services.Abstractions.Providers;
using Realtorist.Web.Models.Attributes;
using Realtorist.Web.Helpers;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Realtorist.Web.Admin.Application.Controllers
{
    /// <summary>
    /// Provides operations related to settings
    /// </summary>
    [ApiController]
    [Route("api/admin/settings")]
    [RequireAuthorization]
    public class SettingsApiController : Controller
    {
        private readonly ISettingsDataAccess _settingsDataAccess;
        private readonly ICachedSettingsProvider _cachedSettingsProvider;
        private readonly IEncryptionProvider _encryptionProvider;
        private readonly ILogger _logger;

        public SettingsApiController(ISettingsDataAccess settingsDataAccess, IEncryptionProvider encryptionProvider, ILogger<SettingsApiController> logger, ICachedSettingsProvider cachedSettingsProvider = null)
        {
            _settingsDataAccess = settingsDataAccess ?? throw new ArgumentNullException(nameof(settingsDataAccess));
            _encryptionProvider = encryptionProvider ?? throw new ArgumentNullException(nameof(encryptionProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cachedSettingsProvider = cachedSettingsProvider;
        }

        /// <summary>
        /// Gets setting
        /// </summary>
        /// <param name="type">Setting type</param>
        /// <returns>Setting</returns>
        [HttpGet]
        [Route("{type}")]
        public async Task<JsonResult> GetSetting([FromRoute] string type)
        {
            var setting = await _settingsDataAccess.GetSettingAsync(type);
            if (setting is null) return Json(null);

            JToken json = JsonConvert.DeserializeObject<JToken>(JsonConvert.SerializeObject(setting));
            WalkNode(json, node =>
            {
                foreach(var property in node.Properties())
                {
                    _logger.LogInformation($"Found property: {property.Name} = {property.Value}       Type: {property.Type}");
                }
                foreach (var property in GetPasswordProperties(node))
                {
                    property.Value = _encryptionProvider.EncryptTwoWay(((string)property.Value));
                }
            });

            return Json(json, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        /// <summary>
        /// Updates setting
        /// </summary>
        /// <param name="type">Setting type</param>
        /// <param name="value">New setting value</param>
        /// <returns>OK</returns>
        [HttpPost]
        [Route("{type}")]
        public async Task<IActionResult> UpdateSetting([FromRoute] string type, [FromBody] JToken value)
        {
            if (value is null) return BadRequest();

            WalkNode(value, node => {
                foreach (var property in GetPasswordProperties(node))
                {
                    if (property.Value == null) continue;
                    try {
                        property.Value = _encryptionProvider.Decrypt((string)property.Value);
                    }
                    catch {
                        property.Value = _encryptionProvider.EncryptTwoWay(((string)property.Value));
                    }
                }
            });

            var convertType = typeof(ExpandoObject);
            if (value.Type == JTokenType.Array)
            {
                convertType = typeof(ExpandoObject[]);
            }
            
            if (SettingTypes.SettingTypeMap.ContainsKey(type))
            {
                ModelState.Clear();
                if (!TryValidateModel(value.ToObject(SettingTypes.SettingTypeMap[type])))
                {
                    return BadRequest(ModelState.GetModelStateValidationErrors());
                }
            }

            var data = value.ToObject(convertType);

            await _settingsDataAccess.UpdateSettingsAsync(type, data);

            _cachedSettingsProvider?.ResetSettingCache(type);

            return NoContent();
        }

        private static void WalkNode(JToken node, Action<JObject> action)
        {
            if (node.Type == JTokenType.Object)
            {
                action((JObject)node);

                foreach (JProperty child in node.Children<JProperty>())
                {
                    WalkNode(child.Value, action);
                }
            }
            else if (node.Type == JTokenType.Array)
            {
                foreach (JToken child in node.Children())
                {
                    WalkNode(child, action);
                }
            }
        }

        private static IEnumerable<JProperty> GetPasswordProperties(JObject node) 
            => node.Properties().Where(p => p.Type == JTokenType.Property 
                && p.Name.ToLower().Contains("password")
                && p.Value != null
                && p.Value.Type == JTokenType.String);
    }
}