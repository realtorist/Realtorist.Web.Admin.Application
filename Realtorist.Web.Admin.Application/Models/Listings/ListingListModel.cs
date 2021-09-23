using System;
using System.ComponentModel.DataAnnotations;
using Realtorist.Models.Enums.LookupTypes;
using Realtorist.Models.Listings.Details;
using Realtorist.Models.Listings.Enums;

namespace Realtorist.Web.Admin.Application.Models.Listings
{
    /// <summary>
    /// Describes the list model for listing
    /// </summary>
    public class ListingListModel
    {
        /// <summary>
        /// Listing Id. Identical to <see cref="PropertyDetails.ID"/>
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Marks the property as featured
        /// </summary>
        public bool Featured { get; set; }

        /// <summary>
        /// Hides the property from search
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Indicates whether listing was created by user or is from MLS
        /// </summary>
        public ListingSource Source { get; set; }

        /// <summary>
        /// The MLS Number of the property.
        /// </summary>
        [Display(Name = "MLS® Number")]
        public string MlsNumber { get; set; }

        /// <summary>
        /// (Attribute) The date the property was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Address of property
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Type of the transaction
        /// </summary>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// The type of ownership ex: Condo/Strata, freehold etc.
        /// </summary>
        [Display(Name = "Title")]
        public OwnershipType? OwnershipType { get; set; }

        /// <summary>
        /// Photos of the property
        /// </summary>
        public Photo[] Photos { get; set; }

        /// <summary>
        /// Price of the Property
        /// </summary>
        public double? Price { get; set; }

        /// <summary>
        /// Lease time e.g. quarterly
        /// </summary>
        public PaymentUnit? PricePerTime { get; set; }

        /// <summary>
        /// Lease unit e.g. square feet
        /// </summary>
        public MeasureUnit? PricePerUnit { get; set; }

        /// <summary>
        /// Type of real estate property
        /// </summary>
        public PropertyType? PropertyType { get; set; }

        /// <summary>
        /// Number of views
        /// </summary>
        public int Views { get; set; }
    }
}
