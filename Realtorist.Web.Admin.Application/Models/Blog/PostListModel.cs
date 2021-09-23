using System;
using System.ComponentModel.DataAnnotations;

namespace Realtorist.Web.Admin.Application.Models.Blog
{
    /// <summary>
    /// Describes list model for the blog post
    /// </summary>
    public class PostListModel
    {
        /// <summary>
        /// Post ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Post link
        /// </summary>
        [Display(Name ="Link")]
        public string Link { get; set; }

        /// <summary>
        /// Post title
        /// </summary>
        [Display(Name ="Title")]
        public string Title { get; set; }

        /// <summary>
        /// Post sub title
        /// </summary>
        [Display(Name ="Sub title")]
        public string SubTitle { get; set; }

        /// <summary>
        /// Post title image
        /// </summary>
        [Display(Name ="Featured Image")]
        public string Image { get; set; }

        /// <summary>
        /// Publish date and time
        /// </summary>
        [Display(Name ="Publish Date")]
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// Category of the post
        /// </summary>
        [Required]
        [Display(Name ="Category")]
        public string Category { get; set; }

        /// <summary>
        /// Tags of the post
        /// </summary>
        [Display(Name ="Tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// Comments to the post
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        /// How many times this post was viewed in full
        /// </summary>
        public int Views { get; set; }
    }
}