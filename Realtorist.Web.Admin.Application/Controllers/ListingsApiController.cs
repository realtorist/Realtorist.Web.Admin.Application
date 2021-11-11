using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Realtorist.DataAccess.Abstractions;
using Realtorist.GeoCoding.Abstractions;
using Realtorist.Models.Events;
using Realtorist.Models.Helpers;
using Realtorist.Models.Listings;
using Realtorist.Models.Pagination;
using Realtorist.Models.Settings;
using Realtorist.RetsClient.Abstractions;
using Realtorist.Services.Abstractions.Events;
using Realtorist.Web.Models.Attributes;
using Realtorist.Web.Models.Abstractions.Jobs.Background;
using Realtorist.Web.Helpers;
using Realtorist.Web.Admin.Application.Models.Listings;
using Realtorist.Extensions.Base.Manager;
using System.Linq;
using Realtorist.Services.Abstractions.Providers;

namespace Realtorist.Web.Admin.Application.Controllers
{
    /// <summary>
    /// Provides operations related to pages
    /// </summary>
    [ApiController]
    [Route("api/admin/listings")]
    [RequireAuthorization]
    public class ListingsApiController : Controller
    {
        private readonly IListingsDataAccess _listingsDataAccess;
        private readonly ISettingsDataAccess _settingsDataAccess;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IEncryptionProvider _encryptionProvider;
        private readonly IExtensionManager _extensionManager;
        private readonly IGeoCoder _geoCoder;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ListingsApiController(
            IListingsDataAccess listingsDataAccess,
            ISettingsDataAccess settingsDataAccess,
            IGeoCoder geoCoder,
            IMapper mapper,
            IBackgroundTaskQueue backgroundTaskQueue,
            IEncryptionProvider encryptionProvider,
            IExtensionManager extensionManager,
            ILogger<ListingsApiController> logger)
        {
            _listingsDataAccess = listingsDataAccess ?? throw new ArgumentNullException(nameof(listingsDataAccess));
            _settingsDataAccess = settingsDataAccess ?? throw new ArgumentNullException(nameof(settingsDataAccess));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _geoCoder = geoCoder ?? throw new ArgumentNullException(nameof(geoCoder));
            _backgroundTaskQueue = backgroundTaskQueue ?? throw new ArgumentNullException(nameof(backgroundTaskQueue));
            _extensionManager = extensionManager ?? throw new ArgumentNullException(nameof(extensionManager));
            _encryptionProvider = encryptionProvider ?? throw new ArgumentNullException(nameof(encryptionProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets listings
        /// </summary>
        /// <param name="request">Pagination request</param>
        /// <param name="filter">Search filters</param>
        /// <returns>Listings</returns>
        [Route("")]
        [HttpGet]
        public async Task<PaginationResult<ListingListModel>> GetListingsAsync([FromQuery] PaginationRequest request, [ModelBinder(BinderType = typeof(QueryStringToDictionaryBinder))] Dictionary<string, string> filter)
        {
            if (request != null && request.SortField.IsNullOrEmpty())
            {
                request.SortField = nameof(ListingListModel.LastUpdated);
                request.SortOrder = Realtorist.Models.Enums.SortByOrder.Desc;
            }

            var feeds = (await _settingsDataAccess.GetSettingAsync<ListingsSettings>(SettingTypes.Listings))?.Feeds ?? new ListingsFeed[0];

            var listings = await _listingsDataAccess.GetListingsAsync<ListingListModel>(request, filter);
            foreach (var listing in listings.Results)
            {
                if (listing.FeedId is null) continue;
                listing.FeedType = feeds.FirstOrDefault(f => f.Id == listing.FeedId)?.FeedType ?? "Unknown";
            }

            return listings;
        }

        /// <summary>
        /// Gets listing by id
        /// </summary>
        /// <param name="listingId">Id of the listing</param>
        /// <returns>Listing</returns>
        [Route("{listingId}")]
        [HttpGet]
        public async Task<Listing> GetListingAsync([FromRoute] Guid listingId)
        {
            var listing = await _listingsDataAccess.GetListingAsync(listingId);
            return listing;
        }

        /// <summary>
        /// Updates listing by id
        /// </summary>
        /// <param name="listingId">Id of the listing to update</param>
        /// <param name="listing">Listing new update model</param>
        /// <returns>Status code</returns>
        [Route("{listingId}")]
        [HttpPost]
        public async Task<IActionResult> UpdateListingAsync([FromRoute] Guid listingId, [FromBody] Listing listing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetModelStateValidationErrors());
            }
            
            var oldListing = await _listingsDataAccess.GetListingAsync(listingId);
            if (oldListing.FeedId is not null)
            {
                return BadRequest($"Listing {listingId} can't be updated as it's comming from the MLS.");
            }

            listing.FeedId = null;
            listing.FeedType = null;
            listing.LastUpdated = DateTime.Now;

            var coordinates = await _geoCoder.GetCoordinatesAsync(listing.Address);
            if (coordinates.IsNullOrEmpty())
            {
                return BadRequest($"Can't get coordinates for the provided address");
            }

            listing.Address.Coordinates = coordinates;
            
            await _listingsDataAccess.UpdateOrAddListingAsync(listingId, listing);
            return NoContent();
        }

        /// <summary>
        /// Creates new listing
        /// </summary>
        /// <param name="page">New listing</param>
        /// <returns>Listing Id</returns>
        [Route("")]
        [HttpPut]
        public async Task<ActionResult<Guid>> CreateListingAsync([FromBody] Listing listing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetModelStateValidationErrors());
            }

            listing.Id = Guid.NewGuid();
            listing.FeedId = null;
            listing.FeedType = null;
            listing.LastUpdated = DateTime.Now;

            var coordinates = await _geoCoder.GetCoordinatesAsync(listing.Address);
            if (coordinates.IsNullOrEmpty())
            {
                return BadRequest($"Can't get coordinates for the provided address");
            }

            listing.Address.Coordinates = coordinates;
            
            await _listingsDataAccess.AddNewListingAsync(listing);
            return new ObjectResult(listing.Id);
        }

        /// <summary>
        /// Deletes listing by id
        /// </summary>
        /// <param name="listingId">Id of the listing</param>
        /// <returns>No content</returns>
        [Route("{listingId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteListingAsync([FromRoute] Guid listingId)
        {
            var listing = await _listingsDataAccess.GetListingAsync(listingId);
            if (listing.FeedId is not null)
            {
                return BadRequest($"Listing {listingId} can't be removed as it's comming from the MLS.");
            }

            await _listingsDataAccess.RemoveListingsAsync(listingId);
            return NoContent();
        }

        /// <summary>
        /// Marks listing as feautered
        /// </summary>
        /// <param name="listingId">Id of the listing</param>
        /// <returns>No content</returns>
        [Route("{listingId}/feature")]
        [HttpPost]
        public async Task<IActionResult> MarkListingAsFeaturedAsync([FromRoute] Guid listingId)
        {            
            await _listingsDataAccess.MarkListingAsFeaturedAsync(listingId, true);
            return NoContent();
        }

        /// <summary>
        /// Marks listing as unfeautered
        /// </summary>
        /// <param name="listingId">Id of the listing</param>
        /// <returns>No content</returns>
        [Route("{listingId}/unfeature")]
        [HttpPost]
        public async Task<IActionResult> MarkListingAsUnFeaturedAsync([FromRoute] Guid listingId)
        {            
            await _listingsDataAccess.MarkListingAsFeaturedAsync(listingId, false);
            return NoContent();
        }

        /// <summary>
        /// Marks listing as disable
        /// </summary>
        /// <param name="listingId">Id of the listing</param>
        /// <returns>No content</returns>
        [Route("{listingId}/disable")]
        [HttpPost]
        public async Task<IActionResult> MarkListingAsDisabledAsync([FromRoute] Guid listingId)
        {            
            await _listingsDataAccess.MarkListingAsDisabledAsync(listingId, true);
            return NoContent();
        }

        /// <summary>
        /// Marks listing as enabled
        /// </summary>
        /// <param name="listingId">Id of the listing</param>
        /// <returns>No content</returns>
        [Route("{listingId}/enable")]
        [HttpPost]
        public async Task<IActionResult> MarkListingAsEnabledAsync([FromRoute] Guid listingId)
        {            
            await _listingsDataAccess.MarkListingAsDisabledAsync(listingId, false);
            return NoContent();
        }

        /// <summary>
        /// Launches listings update from the specified configuration
        /// </summary>
        /// <param name="configuration">RETS configuration</param>
        /// <returns>Ok</returns>
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> LaunchUpdateAsync([FromBody] ListingsFeed configuration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetModelStateValidationErrors());
            }

            var type = _extensionManager.GetInstances<IListingsFeedExtension>().FirstOrDefault(ext => ext.Name == configuration.FeedType)?.UpdateFlowType;
            if (type is null)
            {
                return BadRequest($"Unknown feed type: {configuration.FeedType}");
            }

            try {
                configuration.Password = _encryptionProvider.Decrypt(configuration.Password);
            } catch {
                configuration.Password = _encryptionProvider.EncryptTwoWay(configuration.Password);
            }

            _backgroundTaskQueue.EnqueueTask(async (serviceScopeFactory, cancellationToken) =>
            {
                // Get services
                using var scope = serviceScopeFactory.CreateScope();

                var flow = (IUpdateFlow)ActivatorUtilities.CreateInstance(scope.ServiceProvider, type, Options.Create(configuration));
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<ListingsApiController>>();
                var eventLogger = scope.ServiceProvider.GetRequiredService<IEventLogger>();

                try
                {
                    await flow.LaunchAsync();
                }
                catch (Exception ex)
                {
                    var logMessage = $"Failed to update listings from {configuration.FeedType}";
                    logger.LogError(ex, logMessage);
                    await eventLogger.CreateEventAsync(EventTypes.Generic, logMessage, logMessage, ex);
                }
            });

            _logger.LogInformation($"Successfully put a task for listing update from source '{configuration.FeedType}' into the queue.");

            return Ok();
        }

        [HttpGet]
        [Route("feed/types")]
        public string[] GetFeedTypes()
        {
            return _extensionManager.GetInstances<IListingsFeedExtension>().Select(feed => feed.Name).ToArray();
        }
    }
}