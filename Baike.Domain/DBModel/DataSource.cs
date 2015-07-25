using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace Baike.Entity{
    using System.Linq;

    /// <summary>
    /// The data source.
    /// </summary>
    public class DataSource : BaseEntity
    {
        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        public int WebsiteId{ get; set; }

        /// <summary>
        /// Gets or sets the kewords.
        /// </summary>
        public string Kewords { get; set; }

        /// <summary>
        /// Gets or sets the into url.
        /// </summary>
        public string IntoUrl { get; set; }

        /// <summary>
        /// Gets or sets the logo.
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// Gets or sets the content regex.
        /// </summary>
        public string ContentRegEx { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the web site info.
        /// </summary>
        public WebSite WebSiteInfo { get; set; }

        public string Encode { get; set; }

        public string Removewords { get; set; }

        public List<string> RemovewordsList {
            get
            {
                if (!string.IsNullOrEmpty(this.Removewords))
                {
                    var words = this.Removewords.Split(',');

                    if (words.Length > 0)
                    {
                        return words.ToList();
                    }
                }

                return new List<string>();
            }
        }
    }
}

