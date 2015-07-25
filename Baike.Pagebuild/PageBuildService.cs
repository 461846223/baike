using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Infrastructure;
using VTime.CMS.Data.Domain;
using VTime.CMS.Data.Services.Content;
using VTime.CMS.PageBuild.Models;

namespace VTime.CMS.PageBuild
{
    using Baike.Pagebuild.Models;

    public class PageBuildService
    {
        private IContentService contentService = EngineContext.Current.Resolve<IContentService>();
        private INodeService nodeService = EngineContext.Current.Resolve<INodeService>();
        private int pagesize = 30;

        private string basepath;
        private string baseurl="http://c.shijianbao.com.cn";

        public PageBuildService(string basepath)
        {
            this.basepath = basepath;
        }



        public void Main()
        {
          var allnodes=  nodeService.GetAllNode(0, 100000);
            if (allnodes != null && allnodes.Count > 0)
            {
                foreach (var node in allnodes)
                {
                    this.BuildListPages(node.Id);
                    this.BuildContents(node.Id);
                }
            }
        }

        private void Contents(int id)
        {
            var content = this.contentService.GetContentById(id);
            this.BuildContentHtml(content);
        }

        private void BuildContents(int nodeid)
        {
            var contents = this.contentService.GetAllContent(nodeid, 0, pagesize);
            foreach (var item in contents)
            {
                this.BuildContentHtml(item);
            }

            if (contents.TotalPages >= 2)
            {
                for (int i = 1; i < contents.TotalPages; i++)
                {
                    contents = this.contentService.GetAllContent(nodeid, i, pagesize);
                    foreach (var item in contents)
                    {
                        this.BuildContentHtml(item);
                    }
                }
            }
        }

        private void BuildListPage()
        {

            var nodes = nodeService.GetAllNode(0, 100);

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    
                }
            }
        }

        /// <summary>
        /// 生成列表页
        /// </summary>
        /// <param name="nodeid"></param>
        private void BuildListPages(int nodeid)
        {
            var contents = this.contentService.GetAllContent(nodeid, 0, pagesize);

       
            ////第一页
            this.BuildListPage(contents,nodeid,1);


            if (contents.TotalPages >= 2)
            {
                for (int i = 1; i < contents.TotalPages; i++)
                {
                    contents = this.contentService.GetAllContent(nodeid, i, pagesize);
                   this.BuildListPage(contents,nodeid,i+1);
                }
            }
        }

        private void BuildListPage(IPagedList<ContentItem> items, int nodeid, int pageindex)
        {
            var model = new ListPageModel();
            model.Nodeid = nodeid;
            model.Pageindex = pageindex;
            model.Nextpage = items.HasNextPage ? BuildListpageUrl(nodeid,pageindex+1) : string.Empty;

            foreach (var item in items)
            {
                model.Items.Add(new ContentItemModel()
                {
                    Title = item.Title,
                    ImageUrl = item.ImageUrl,
                    Nodeid = item.NodeId,
                    Summary = item.Summary,
                    Url = BuildItemulr(item)
                });
            }

            var text = FileHelper.ReadInExecutingAssembly("views\\list.cshtml");

            var htmltext = RazorEngine.Razor.Parse(text, model);

            FileHelper.Create(string.Format("{0}\\{1}\\{2}.htm", this.basepath, nodeid,
                pageindex), htmltext, Encoding.UTF8);
        }




        private void BuildContentHtml(ContentItem content)
        {
            if (content != null)
            {
                var model = new ContentPageModel();
                model.Title = content.Title;
                model.ContentText = content.ContentText;
                model.NodeID = content.NodeId;
                model.SourceUrl = content.SourceUrl;
                model.AddDate = content.AddDate;
                if (content.NodeInfo != null)
                {
                    model.NodeName = content.NodeInfo.Title;
                    model.Nodeurl = this.BuildListpageUrl(content.NodeId, 1);
                }

                var text = FileHelper.ReadInExecutingAssembly("views\\content.cshtml");

                var html = RazorEngine.Razor.Parse(text, model);

                FileHelper.Create(string.Format("{0}\\c\\{1}\\{2}.htm", this.basepath, content.AddDate.ToString("yyyyMMdd"),
                    content.Id), html, Encoding.UTF8);
            }
        }

        private string BuildItemulr(ContentItem item)
        {
            if (item != null)
            {
                return string.Format("{0}/c/{1}/{2}.htm", this.baseurl, item.AddDate.ToString("yyyyMMdd"), item.Id);
            }

            return string.Empty;
        }

        private string BuildListpageUrl(int nodeid,int pageindx)
        {
            return string.Format("{0}/{1}/{2}.htm", baseurl, nodeid, pageindx);
        }
    }
}
