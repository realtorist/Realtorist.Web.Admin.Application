namespace Realtorist.Web.Admin.Application.Models.Links
{
    /// <summary>
    /// Describes link available on the website
    /// </summary>
    public class SiteLink
    {
        public SiteLink()
        {
        }

        public SiteLink(string link, string title)
        {
            this.link = link;
            Title = title;
        }

        /// <summary>
        /// Link
        /// </summary>
        public string link { get; set; }

        /// <summary>
        /// Title of the link
        /// </summary>
        public string Title { get; set; }
    }
}