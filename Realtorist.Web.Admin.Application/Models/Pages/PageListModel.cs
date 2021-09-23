using System;

namespace Realtorist.Models.Page
{
    /// <summary>
    /// Describes list model user-created page 
    /// </summary>
    public class PageListModel
    {
        /// <summary>
        /// Id of the page
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Indicates whether page is hidden for visitors
        /// </summary>
        public bool UnPublished { get; set; }

        /// <summary>
        /// Link of the page
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Title of the page
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Number of page views
        /// </summary>
        public int Views { get; set; }
    }
}