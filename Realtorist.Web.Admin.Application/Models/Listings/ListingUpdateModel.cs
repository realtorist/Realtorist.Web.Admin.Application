// using System;
// using Realtorist.Models.Enums.LookupTypes;
// using Realtorist.Models.Models;

// namespace Realtorist.Web.Admin.Application.Models.Listings
// {
//     public class ListingUpdateModel
//     {
//         /// <summary>
//         /// The date the property was last updated
//         /// </summary>
//         public DateTime? LastUpdated { get; set; }

//         /// <summary>
//         /// The MLS Number of the property.
//         /// </summary>
//         public string ListingID { get; set; }

//         /// <summary>
//         /// Building details of the property
//         /// </summary>
//         public Building Building { get; set; }

//         /// <summary>
//         /// Details about the land of the property
//         /// </summary>
//         public Land Land { get; set; }

//         /// <summary>
//         /// Address of property
//         /// </summary>
//         public Address Address { get; set; }

//         /// <summary>
//         /// Set of links to information about the property
//         /// </summary>
//         public AlternateURL AlternateURL { get; set; }

//         /// <summary>
//         /// List of amenities nearby to the property
//         /// </summary>
//         public AmenitiesNearby[] AmmenitiesNearBy { get; set; }

//         /// <summary>
//         /// The community features
//         /// </summary>
//         public CommunityFeatures[] CommunityFeatures { get; set; }

//         /// <summary>
//         /// Lease value
//         /// </summary>
//         public double? Lease { get; set; }

//         /// <summary>
//         /// Lease time e.g. quarterly
//         /// </summary>
//         public PaymentUnit? LeasePerTime { get; set; }

//         /// <summary>
//         /// Lease unit e.g. square feet
//         /// </summary>
//         public MeasureUnit? LeasePerUnit { get; set; }

//         /// <summary>
//         /// A description of the location
//         /// </summary>
//         public string LocationDescription { get; set; }

//         /// <summary>
//         /// Condo/Maintenances fees
//         /// </summary>
//         public string MaintenanceFee { get; set; }

//         /// <summary>
//         /// Condo/Maintenances fee payment unit (Frequency)
//         /// </summary>
//         public string MaintenanceFeePaymentUnit { get; set; }

//         /// <summary>
//         /// Condo/Maintenances fee type
//         /// </summary>
//         public MaintenanceFeeType[] MaintenanceFeeType { get; set; }

//         /// <summary>
//         /// Collection of open house Events
//         /// </summary>
//         public OpenHouse OpenHouse { get; set; }

//         /// <summary>
//         /// Collection of Live Streame Open House events
//         /// </summary>
//         public LiveStream LiveStream { get; set; }

//         /// <summary>
//         /// Collection of Parking. Available if parking type details are available
//         /// </summary>
//         public ParkingSpaces ParkingSpaces { get; set; }

//         /// <summary>
//         /// The total number of parking spaces
//         /// </summary>
//         public int? ParkingSpaceTotal { get; set; }

//         /// <summary>
//         /// Collection of PropertyPhoto
//         /// </summary>
//         public Photo Photo { get; set; }

//         /// <summary>
//         /// Price of the Property
//         /// </summary>
//         public double? Price { get; set; }

//         /// <summary>
//         /// Type of real estate property
//         /// </summary>
//         public PropertyType? PropertyType { get; set; }

//         /// <summary>
//         /// Description of the property
//         /// </summary>
//         public string PublicRemarks { get; set; }

//         /// <summary>
//         /// Type of transaction (e.g. sale, lease)
//         /// </summary>
//         public TransactionType? TransactionType { get; set; }

//         /// <summary>
//         /// Types of views available
//         /// </summary>
//         public ViewType[] ViewType { get; set; }

//         /// <summary>
//         /// Waterfront type of the property
//         /// </summary>
//         public WaterFrontType? WaterFrontType { get; set; }

//         /// <summary>
//         /// Name of the waterfront the property is on
//         /// </summary>
//         public string WaterFrontName { get; set; }

//         /// <summary>
//         /// Property Zoning type
//         /// </summary>
//         public ZoningType? ZoningType { get; set; }
//     }
// }