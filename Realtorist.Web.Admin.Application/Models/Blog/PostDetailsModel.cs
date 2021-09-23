using System;
using System.ComponentModel.DataAnnotations;

namespace Realtorist.Web.Admin.Application.Models.Blog
{
    /// <summary>
    /// Describes details model for the blog post
    /// </summary>
    public class PostDetailsModel : PostListModel
    {
        /// <summary>
        /// Post content
        /// </summary>
        [Display(Name ="Text")]
        public string Text { get; set; }
    }
}