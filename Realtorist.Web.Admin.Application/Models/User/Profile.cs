using System.ComponentModel.DataAnnotations;

namespace Realtorist.Web.Admin.Application.Models.User
{
    /// <summary>
    /// Describes user profile
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// User's first name
        /// </summary>
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        /// <summary>
        /// User's last name
        /// </summary>
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        /// <summary>
        /// User's full name
        /// </summary>
        /// <value></value>
        [Display(Name = "Full name")]
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Website's name
        /// </summary>
        [Display(Name = "Website name")]
        public string WebsiteName { get; set; }

        /// <summary>
        /// Website's title
        /// </summary>
        [Display(Name = "Website title")]
        public string WebsiteTitle { get; set; }

        /// <summary>
        /// Website's URL address
        /// </summary>
        [Display(Name = "Website address")]
        public string WebsiteAddress { get; set; }

        /// <summary>
        /// User's email
        /// </summary>
        [Display(Name = "Your email")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// User's phone
        /// </summary>
        [Display(Name = "Your phone")]
        public string Phone { get; set; }

        /// <summary>
        /// User's physical address
        /// </summary>
        [Display(Name = "Your address")]
        public string Address { get; set; }

        /// <summary>
        /// User's avatar URL
        /// </summary>
        [Display(Name = "URL to your picture")]
        public string Avatar { get; set; }

        /// <summary>
        /// User's brief description
        /// </summary>
        /// <value></value>
        [Display(Name = "Brief description of you")]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Theme of the admin panel
        /// </summary>
        public string AdminTheme { get; set; }
    }
}