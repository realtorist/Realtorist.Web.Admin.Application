using Realtorist.Models.Blog;
using Realtorist.Models.Listings;
using Realtorist.Models.CustomerRequests;
using Realtorist.Models.Page;
using Realtorist.Models.Settings;
using Realtorist.Web.Admin.Application.Models.User;
using Realtorist.Web.Admin.Application.Models.Blog;
using Realtorist.Web.Admin.Application.Models.CustomerRequests;
using Realtorist.Web.Admin.Application.Models.Listings;

namespace Realtorist.Web.Admin.Application
{
    /// <summary>
    /// Defines mapper profile for models
    /// </summary>
    public class AutoMapperProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Default ctr
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<WebsiteSettings, UserProfile>()
                .ForMember(dest => dest.WebsiteAddress, dest => dest.MapFrom(source => source.WebsiteAddress))
                .ForMember(dest => dest.WebsiteName, dest => dest.MapFrom(source => source.WebsiteName))
                .ForMember(dest => dest.WebsiteTitle, dest => dest.MapFrom(source => source.WebsiteTitle))
                .ForAllOtherMembers(map => map.Ignore());

            CreateMap<ProfileSettings, UserProfile>()
                .ForMember(dest => dest.WebsiteAddress, dest => dest.Ignore())
                .ForMember(dest => dest.WebsiteName, dest => dest.Ignore())
                .ForMember(dest => dest.WebsiteTitle, dest => dest.Ignore());

            CreateMap<Post, PostListModel>().ForMember(dest => dest.CommentsCount, dest => dest.MapFrom(source => source.Comments.Length));
            CreateMap<Post, PostDetailsModel>().ForMember(dest => dest.CommentsCount, dest => dest.MapFrom(source => source.Comments.Length));
            CreateMap<Post, PostUpdateModel>().ReverseMap();

            CreateMap<Page, PageListModel>();
            CreateMap<Page, PageUpdateModel>().ReverseMap();

            CreateMap<CustomerRequest, CustomerRequestListModel>()
                .ForMember(dest => dest.ListingId, dest => dest.MapFrom(source => source.Request.ListingId))
                .ForMember(dest => dest.Address, dest => dest.MapFrom(source => source.Request.Address))
                .ForMember(dest => dest.Name, dest => dest.MapFrom(source => source.Request.Name))
                .ForMember(dest => dest.Phone, dest => dest.MapFrom(source => source.Request.Phone))
                .ForMember(dest => dest.Email, dest => dest.MapFrom(source => source.Request.Email))
                .ForMember(dest => dest.Type, dest => dest.MapFrom(source => source.Request.Type))
                .ForMember(dest => dest.Iam, dest => dest.MapFrom(source => source.Request.Iam))
                .ForMember(dest => dest.RepliesCount, dest => dest.MapFrom(source => source.Replies.Length));

            CreateMap<Listing, ListingListModel>()
                .ForMember(dest => dest.Address,
                    dest => dest.MapFrom(source =>
                        source.Address.StreetAddress
                        + ", "
                        + source.Address.City
                        + ", "
                        + source.Address.Province
                        + ", "
                        + source.Address.PostalCode));
        }
    }
}