using System.ComponentModel.DataAnnotations;

namespace Realtorist.Web.Admin.Application.Models.Auth
{
    public class LoginModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}