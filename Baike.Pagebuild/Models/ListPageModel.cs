using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Baike.Pagebuild.Models
{
    using Baike.Entity;
    using HQ.Common.Paging;

    public class ListPageModel : BaseModel
    {
        public ListPageModel()
        {
         
        }

        public Node NodeoInfo { get; set; }

        /// <summary>
        /// Gets or sets the page items.
        /// </summary>
        public PagedList<Content> PageItems { get; set; }

    }

    public class ContentItemModel
    {

        public int Nodeid { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string ImageUrl { get; set; }
    }

}
