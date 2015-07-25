using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;

namespace Baike.Entity
{
    using System.Linq;

    /// <summary>
    /// The web site.
    /// </summary>
    public class WebSite : BaseEntity
    {
        /// <summary>
        /// Gets or sets the site name.
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// 备案号
        /// </summary>
        public string RecordNumber { get; set; }

        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the domain name.
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// PhysicalPath
        /// </summary>
        public string PhysicalPath { get; set; }

        /// <summary>
        /// Gets or sets the websitedatasouces.
        /// </summary>
        public virtual ICollection<DataSource> DataSouces { get; set; }

        public string Title { get; set; }
    }
}

