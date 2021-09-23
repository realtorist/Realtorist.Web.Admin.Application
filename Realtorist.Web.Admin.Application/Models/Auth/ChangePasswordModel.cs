using System;
using System.ComponentModel.DataAnnotations;

namespace Realtorist.Web.Admin.Application.Models.Auth
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }

        [MinLength(6)]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}