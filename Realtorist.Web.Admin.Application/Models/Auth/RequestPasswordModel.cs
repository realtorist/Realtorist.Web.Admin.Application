using System.ComponentModel.DataAnnotations;

namespace Realtorist.Web.Admin.Application.Models.Auth
{
    public class RequestPasswordModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}