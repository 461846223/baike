using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baike.Pagebuild
{
    using System.Configuration;
    using System.IO;

    using Baike.Pagebuild.Models;

    using HQ.Common;
    using HQ.Common.Paging;

    /// <summary>
    /// The content controller.
    /// </summary>
    public class ContentController : BaseController
    {
        private static Random random = new Random();

        public ContentController(int siteid)
            : base(siteid)
        {
            this.Template = HQ.Common.FileLogic.ReadInExecutingAssembly("views/content/index.html");
            if (string.IsNullOrEmpty(this.Template))
            {
                throw new Exception("Content Template Cannot is null");
            }
        }


        public void BuildAllContent()
        {
            var nodes = this.unitOfWork.NodeRepository.Get(c => c.SiteId == this.Siteinfo.Id);
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    this.BuildContent(node.Id);
                }
            }
        }



        public void BuildContent(int nodeid)
        {
            int pageindex = 1;
            int pagesize = 30;
            int total;
            var items = this.unitOfWork.ContentRepository.Get(
                c => c.NodeId == nodeid,
                out total,
                0,
                pagesize,
                or => or.OrderByDescending(o => o.Id));

            if (items != null)
            {
                foreach (var content in items)
                {
                    this.Index(content.Id);
                }

                var pageitems = items.ToPagedList(0, pagesize, total);

                if (pageitems.PageCount > 1)
                {
                    for (int i = 1; i < pageitems.PageCount; i++)
                    {
                        pageindex = i;

                        items = this.unitOfWork.ContentRepository.Get(
                            c => c.NodeId == nodeid,
                            out total,
                            pageindex,
                            pagesize,
                            or => or.OrderByDescending(o => o.Id));

                        foreach (var content in items)
                        {
                            this.Index(content.Id);
                        }
                    }
                }
            }
        }



        public void Index(int id)
        {
            try
            {
               

                var model = new ContentPageModel();
            
                var content = this.unitOfWork.ContentRepository.Find(id);

                if (content != null)
                {
                    var filename = this.GetContentPath(content.NodeId, content.Id);
                    if (File.Exists(filename))
                    {
                        return;
                    }

                    model.WebSiteInfo = this.Siteinfo;
                    model.SidebarTag = this.SidebarTag;
                    model.NavList = this.GetNavigation(0);

                    ////seo 信息
                    model.SeoInfo.Title = string.Format("{0}_{1}", content.Title, this.Siteinfo.SiteName);
                    model.SeoInfo.Keywords = content.SEOkeywords;
                    model.SeoInfo.Description = content.SEOdescription;

                    model.BreadcrumbNavigation = this.getUrlBreadcrumbNavigation(content.NodeId, content.Id);
                    model.NavList = this.GetNavigation(content.NodeId);
                    model.SidebarTag = this.SidebarTag;

                    model.Title = content.Title;
                    model.Subtitle = content.Subtitle;


                    if (ConfigurationManager.AppSettings["AddKeywordImgToContentText"] == "0")
                    {
                        model.ContentText = content.ContentText;
                    }
                    else
                    {
                        model.ContentText = this.AddKeywordImgToContentText(content.ContentText, content.SEOkeywords);
                    }
                   

                    model.ContentText = HTMLTagsCheck.Fix(model.ContentText);

                    model.NodeID = content.NodeId;

                    if (string.IsNullOrEmpty(model.SeoInfo.Description))
                    {
                        model.SeoInfo.Description = content.Summary;
                    }

                    model.Summary = content.Summary;
                    model.ImageUrlse = content.ImageUrlse;
                    model.PreContentUrl = this.GetContenturl(content.NodeId, content.PreviousId);
                    var nextinfo = this.unitOfWork.ContentRepository.Find(c => c.PreviousId == content.Id);

                    if (nextinfo != null)
                    {
                        model.NextContentUrl = this.GetContenturl(content.NodeId, nextinfo.Id);
                    }

                    ////相关文章
                    int total = 0;
                    var reitems = this.unitOfWork.ContentRepository.Get(
                        c => c.NodeId == content.NodeId,
                        out total,
                        0,
                        10);
                    model.RelatedItems = reitems.ToList();
                    var html = RazorEngine.Razor.Parse(this.Template, model);
                    if (!string.IsNullOrEmpty(html))
                    {
                        FileLogic.Create(this.GetContentPath(content.NodeId, content.Id), html, Encoding.UTF8);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }


        /// <summary>
        /// 根据关键字随机获取图片
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private string GetRandomKeywordImg(string keyword)
        {
            ////690px
            string r = string.Empty;
            var keyimgs = this.unitOfWork.KeywordImgRepository.Get(c => c.Keyword == keyword);
            if (keyimgs != null)
            {
                r = "<img src=\"{0}\"  width=\"{1}px\" alt=\"{2}\" />";
                var keyimgslist = keyimgs.ToList();
                if (keyimgslist != null && keyimgslist.Count > 0)
                {
                    var i = random.Next(0, keyimgslist.Count - 1);
                    var img = keyimgslist[i];
                    r = string.Format(r, img.ImgUrl, img.Width < 690 ? img.Width : 690, img.Title);
                }
                else
                {
                    return r;
                }
            }

            return r;
        }

        private string AddKeywordImgToContentText(string text, string keyword)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keyword))
            {
                return text;
            }

            var img = this.GetRandomKeywordImg(keyword);
            int i = text.IndexOf('。');
            if (i < 0)
            {
                i = text.IndexOf('.');
            }
            if (i < 0)
            {
                i = text.IndexOf('!');
            }
            if (i < 0)
            {
                i = text.IndexOf('！');
            }

            if (i > 0)
            {
                text = text.Insert(i + 1, img);
            }


            return text;
        }

    }
}
