using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
                   _ooOoo_
                  o8888888o
                  88" . "88
                  (| -_- |)
                  O\  =  /O
               ____/`---'\____
             .'  \\|     |//  `.
            /  \\|||  :  |||//  \
           /  _||||| -:- |||||-  \
           |   | \\\  -  /// |   |
           | \_|  ''\---/''  |   |
           \  .-\__  `-`  ___/-. /
         ___`. .'  /--.--\  `. . __
      ."" '<  `.___\_<|>_/___.'  >'"".
     | | :  `- \`.;`\ _ /`;.`/ - ` : | |
     \  \ `-.   \_ __\ /__ _/   .-` /  /
======`-.____`-.___\_____/___.-`____.-'======
                   `=---='
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
         佛祖保佑       永无BUG
*/
namespace Baike.Pagebuild
{
    using System.IO;
    using System.Security.Policy;
    using System.Web.Mvc.Razor;
    using System.Web.Razor.Parser;

    using Baike.Entity;
    using Baike.Pagebuild.Models;
    using Baike.Repository;

    using HQ.Common;
    using HQ.Common.Paging;

    using RazorEngine;

    using Encoding = System.Text.Encoding;

    /// <summary>
    /// The list controller.
    /// </summary>
    public class ListController : BaseController
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ListController"/> class.
        /// </summary>
        /// <param name="siteid">
        /// The siteid.
        /// </param>
        public ListController(int siteid)
            : base(siteid)
        {
            this.Template = HQ.Common.FileLogic.ReadInExecutingAssembly("views/list.html");
        }


        public ListPageModel Index(ListRequest request)
        {
            var model = new ListPageModel();
            model.WebSiteInfo = this.Siteinfo;
            model.SidebarTag = this.SidebarTag;

            ////分类目录信息
            var nodeinfo = this.unitOfWork.NodeRepository.Find(request.Nodeid);
            if (nodeinfo != null)
            {
                model.NodeoInfo = nodeinfo;
                model.NavList = this.GetNavigation(request.Nodeid);

                ////seo 信息
                model.SeoInfo.Title = string.Format(
                    "{0}微信号 第{1}页 - {2}",
                    nodeinfo.Title,
                    request.Pageindex,
                    this.Siteinfo.SiteName);
                if (!string.IsNullOrEmpty(nodeinfo.Keywords))
                {
                    model.SeoInfo.Keywords = nodeinfo.Keywords.Replace("&", string.Empty);
                }

                model.SeoInfo.Description = nodeinfo.Description;

                int total;
                var items = this.unitOfWork.ContentRepository.Get(
                    c => c.NodeId == request.Nodeid,
                    out total,
                    request.Pageindex - 1,
                    request.Pagesize,
                    or => or.OrderByDescending(o => o.Id));

                model.PageItems = items.ToPagedList(request.Pageindex, request.Pagesize, total);
            }


            return model;
        }


        public void BuildAllList()
        {
            var nodes = this.unitOfWork.NodeRepository.Get(c => c.SiteId == Siteinfo.Id);

            if (nodes != null && nodes.Any())
            {
                foreach (var node in nodes)
                {
                    try
                    {


                        var request = new ListRequest();
                        request.Nodeid = node.Id;
                        var model = this.Index(request);

                        if (model != null && model.PageItems != null && model.PageItems.Any())
                        {
                            var html = Razor.Parse(this.Template, model);
                            var filepath = this.GetNodePath(node.Id, request.Pageindex);

                            HQ.Common.FileLogic.Create(filepath, html, Encoding.UTF8);

                            if (model.PageItems.PageCount > 1)
                            {
                                for (int i = 1; i < model.PageItems.PageCount; i++)
                                {
                                    request.Nodeid = node.Id;
                                    request.Pageindex = i;
                                    model = this.Index(request);
                                    html = Razor.Parse(this.Template, model);

                                    filepath = this.GetNodePath(node.Id, request.Pageindex);

                                    HQ.Common.FileLogic.Create(filepath, html, Encoding.UTF8);
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                        Logger.Error(ex);
                    }
                }
            }
        }
    }
}
