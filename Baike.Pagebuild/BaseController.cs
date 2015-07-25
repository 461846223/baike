using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baike.Pagebuild
{
    using System.Globalization;

    using Baike.Entity;
    using Baike.Pagebuild.Models;
    using Baike.Repository;

    public class BaseController
    {
        /// <summary>
        /// 模板
        /// </summary>
        protected string Template { get; set; }

        protected string basepath;

        /// <summary>
        /// 
        /// </summary>
        protected UnitOfWork unitOfWork = new UnitOfWork();

        /// <summary>
        /// 网站所有分类目录
        /// </summary>
        private List<int> theSiteNodes { get; set; }

        /// <summary>
        /// 网站信息
        /// </summary>
        public WebSite Siteinfo { get; set; }

        public SidebarTagModel SidebarTag { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        public BaseController(int siteid)
        {
            this.Siteinfo = this.unitOfWork.WebSiteRepository.Find(siteid);
            this.theSiteNodes=new List<int>();

            var nodes = this.unitOfWork.NodeRepository.Get(c => c.SiteId == this.Siteinfo.Id);

            foreach (var node in nodes)
            {
                this.theSiteNodes.Add(node.Id);
            }

            this.SidebarTag = this.GetSidebarTagModel();
        }

        /// <summary>
        /// 面包屑导航
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="contentid"></param>
        /// <returns></returns>
        protected List<UrlInfo> getUrlBreadcrumbNavigation(int nodeid,int? contentid)
        {
            var r = new List<UrlInfo>();
            
            r.Add(new UrlInfo()
                      {
                          Title = "首页",
                          Herf = this.Siteinfo.DomainName
                      });
            if (nodeid > 0)
            {
                var nodeinfo = this.unitOfWork.NodeRepository.Find(nodeid);
                if (nodeinfo != null && nodeinfo.Id > 0)
                {
                    r.Add(new UrlInfo() { Title = nodeinfo.Title, Herf = this.GetNodeurl(nodeid) });
                }
            }

            if (contentid != null)
            {
                var contentinfo = this.unitOfWork.ContentRepository.Find(contentid.Value);
                if (contentinfo != null)
                {
                    r.Add(
                        new UrlInfo()
                            {
                                Title = contentinfo.Title,
                                Herf = this.GetContenturl(contentinfo.NodeId, contentinfo.Id)
                            });
                }
            }

            return r;
        }

        /// <summary>
        /// 生成节点url
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        protected string GetNodeurl(int nodeid)
        {
            return string.Format("{0}/{1}", this.Siteinfo.DomainName, nodeid);
        }

        /// <summary>
        /// 生成文章url
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="contentid"></param>
        /// <returns></returns>
        protected string GetContenturl(int nodeid, int contentid)
        {
            return string.Format("{0}/content/{1}/{2}.shtml", this.Siteinfo.DomainName, nodeid, contentid);
        }

        /// <summary>
        /// 获取侧栏数据
        /// </summary>
        /// <returns></returns>
        protected SidebarTagModel GetSidebarTagModel()
        {
            var model = new SidebarTagModel();

            int total = 0;
            var items = this.unitOfWork.ContentRepository.Get(
                c => this.theSiteNodes.Contains(c.NodeId),
                out total,
               0,
                10,
                or => or.OrderByDescending(o => o.Id));

            if (items != null && items.Any())
            {
                model.LastItems = items.ToList();
            }

            items = this.unitOfWork.ContentRepository.Get(
                c => c.IsHot == 1 && this.theSiteNodes.Contains(c.NodeId),
                out total,
               0,
                10,
                or => or.OrderByDescending(o => o.Id));

            if (items != null && items.Any())
            {
                model.HotItems = items.ToList();
            }

            model.OldItems = this.GetOldList();

            items = this.unitOfWork.ContentRepository.Get(
              c => this.theSiteNodes.Contains(c.NodeId),
              out total,
             0,
              10,
              or => or.OrderByDescending(o => o.SEOkeywords));

            if (items != null && items.Any())
            {
                model.MiddleList = items.ToList();
            }

            return model;
        }

        /// <summary>
        /// 导航
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        protected List<UrlInfo> GetNavigation(int nodeid)
        {
            var r = new List<UrlInfo>();
            var nodes = this.unitOfWork.NodeRepository.Get(c => c.SiteId ==this.Siteinfo.Id);
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    if (node.Contents != null && node.Contents.Count > 0)
                    {
                        r.Add(
                            new UrlInfo()
                                {
                                    Title = node.Title,
                                    Herf = this.GetNodeurl(node.Id),
                                    Selected = nodeid == node.Id
                                });
                    }
                }
            }

            return r;
        }

        /// <summary>
        /// 获取文章物理路径
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="contentid"></param>
        /// <returns></returns>
        protected string GetContentPath(int nodeid, int contentid)
        {
            return string.Format("{0}\\content\\{1}\\{2}.shtml", this.Siteinfo.PhysicalPath,nodeid, contentid);
        }

        /// <summary>
        /// 获取分类页面物理路径
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="contentid"></param>
        /// <returns></returns>
        protected string GetNodePath(int nodeid, int pageindex)
        {
            return string.Format(
                "{0}/{1}/index{2}.shtml",
                this.Siteinfo.PhysicalPath,
                nodeid,
                pageindex > 1 ? pageindex.ToString(CultureInfo.InvariantCulture) : string.Empty);
        }

        protected string GetHomePath()
        {
            return string.Format("{0}\\index.shtml", this.Siteinfo.PhysicalPath);
        }

        protected virtual Seo GetSeoInfo()
        {
            var r = new Seo();
            r.Title = string.Format("{0}-{1}", this.Siteinfo.SiteName, this.Siteinfo.Title);
            r.Keywords = this.Siteinfo.Keywords;
            r.Description = this.Siteinfo.Description;

            return r;
        }

        protected List<Content> GetOldList()
        {
            var r = new List<Content>();
            int total;
            var items = this.unitOfWork.ContentRepository.Get(
                c => this.theSiteNodes.Contains(c.NodeId),
                out total,
                0,
                10,
                or => or.OrderBy(o => o.Id));

            if (items != null && items.Any())
            {
                r = items.ToList();
            }

            return r;
        }
    }
}
