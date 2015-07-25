using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baike.Entity
{
    public class Content : BaseEntity
    {

        public Content()
       {
           this.AddDate = DateTime.Now;
       }

       public int NodeId { get; set; }
        public string Title { get; set; }

        public string Subtitle { get; set; }
        public string ContentText { get; set; }
        public string Summary { get; set; }
        public string SourceName { get; set; }
        public int SourceID { get; set; }
        public string SourceUrl { get; set; }
        public string SEOkeywords { get; set; }
        public string SEOdescription { get; set; }

        public DateTime AddDate { get; set; }

        public string ImageUrl { get; set; }
        public string ImageUrlse { get; set; }

        public virtual Node NodeInfo { get; set; }

        /// <summary>
        /// 上一个id
        /// </summary>
        public int PreviousId { get; set; }


        public int IsRecommend { get; set; }

        public int IsHot { get; set; }
    }
}
