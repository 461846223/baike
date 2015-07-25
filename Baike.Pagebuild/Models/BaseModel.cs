using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baike.Pagebuild.Models
{
    using System.Dynamic;

    using Baike.Entity;

    public class BaseModel
    {
        public BaseModel()
        {
            this.SidebarTag = new SidebarTagModel();
            this.SeoInfo = new Seo();
            this.FriendlyLinks=new List<UrlInfo>();
            this.NavList=new List<UrlInfo>();
        }

        /// <summary>
        /// 网站信息
        /// </summary>
        public WebSite WebSiteInfo { get; set; }

        /// <summary>
        /// seo信息
        /// </summary>
        public Seo SeoInfo { get; set; }


        /// <summary>
        /// 导航
        /// </summary>
        public List<UrlInfo> NavList { get; set; }

        /// <summary>
        /// 侧栏
        /// </summary>
        public SidebarTagModel SidebarTag { get; set; }

        /// <summary>
        /// 友情链接
        /// </summary>
        public List<UrlInfo> FriendlyLinks { get; set; }
    }


    public class SidebarTagModel
    {
        public SidebarTagModel()
        {
            this.HotItems=new List<Content>();
            this.LastItems=new List<Content>();
        }

        /// <summary>
        /// Gets or sets the last items. 最新内容
        /// </summary>
        public List<Content> HotItems { get; set; }

        /// <summary>
        /// 热门
        /// </summary>
        public List<Content> LastItems { get; set; }

        public List<Content> OldItems { get; set; }

        public List<Content> MiddleList { get; set; }  
    }

    public class Seo
    {
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
    }

}
