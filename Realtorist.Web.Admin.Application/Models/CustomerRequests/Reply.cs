using System.ComponentModel.DataAnnotations;

namespace Realtorist.Web.Admin.Application.Models.CustomerRequests
{
    /// <summary>
    /// Describes reply to the customer request
    /// </summary>
    public class Reply
    {
        /// <summary>
        /// Reply message
        /// </summary>
        [Required]
        public string Message { get; set; }
    }
}
