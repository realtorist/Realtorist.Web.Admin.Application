using System;
using Realtorist.Models.Enums;

namespace Realtorist.Web.Admin.Application.Models.CustomerRequests
{
    /// <summary>
    /// Describes customer request list model
    /// </summary>
    public class CustomerRequestListModel
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Marks whether the request is read
        /// </summary>
        public bool Read { get; set; }

        /// <summary>
        /// Date and time of the request in UTC
        /// </summary>
        public DateTime DateTimeUtc { get; set; }

        /// <summary>
        /// Id of the listing related to this request
        /// </summary>
        public Guid? ListingId { get; set; }

        /// <summary>
        /// Address of the listing related to this request
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Name of the customer
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Phone of the customer
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Customer's email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Type of the request
        /// </summary>
        public RequestType Type { get; set; }

        /// <summary>
        /// Type of the customer
        /// </summary>
        public Iam Iam { get; set; }

        /// <summary>
        /// Replies
        /// </summary>
        public int RepliesCount { get; set; }
    }
}