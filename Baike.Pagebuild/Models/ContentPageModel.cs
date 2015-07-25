using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baike.Pagebuild.Models
{
    using Baike.Entity;

    public class ContentPageModel : BaseModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        
        public string ContentText { get; set; }
        public string Summary { get; set; }

        public string NodeName { get; set; }
        public int NodeID { get; set; }
        public string Nodeurl { get; set; }

        public string SEOkeywords { get; set; }
        public string SEOdescription { get; set; }

        public DateTime AddDate { get; set; }


        public string SourceName { get; set; }
        public int SourceID { get; set; }
        public string SourceUrl { get; set; }

        public List<UrlInfo> BreadcrumbNavigation { get; set; }

        public string PreContentUrl { get; set; }

        public string NextContentUrl { get; set; }


        public string ImageUrlse { get; set; }

        /// <summary>
        /// 相关阅读
        /// </summary>
        public List<Content> RelatedItems { get; set; }

    }
}
