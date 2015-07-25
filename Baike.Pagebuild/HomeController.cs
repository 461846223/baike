using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baike.Pagebuild
{
    using Baike.Entity;
    using Baike.Pagebuild.Models;

    /// <summary>
    /// The home controller.
    /// </summary>
    public class HomeController:BaseController
    {

        public HomeController(int siteid)
            : base(siteid)
        {
            this.Template = HQ.Common.FileLogic.ReadInExecutingAssembly("Views/home/index.html");
        }

        public void Index()
        {
            var model = new HomePageModel();
            model.WebSiteInfo = this.Siteinfo;
            model.SidebarTag = this.SidebarTag;
            model.NavList = this.GetNavigation(0);
            model.SeoInfo = this.GetSeoInfo();

            var nodes = this.unitOfWork.NodeRepository.Get(c => c.SiteId == this.Siteinfo.Id);

            if (nodes != null)
            {
                int num = 0;
                foreach (var node in nodes)
                {
                    if (node == null)
                    {
                        continue;
                    }

                    if (num > 10)
                    {
                        continue;
                    }

                    num++;
                    var nodeinfo = new NodeModel();
                    nodeinfo.NodeInfo = node;
                   
                    int total = 0;
                    var items = this.unitOfWork.ContentRepository.Get(
                        c => c.NodeId == node.Id,
                        out total,
                        0,
                        20,
                        o => o.OrderByDescending(a => a.Id));
                    if (items != null && items.Any())
                    {
                        foreach (var content in items)
                        {
                            nodeinfo.Items.Add(content);
                        }
                    }

                    model.NodeModels.Add(nodeinfo);
                }
            }

            var html = RazorEngine.Razor.Parse(this.Template, model);

            var filepath = GetHomePath();

            HQ.Common.FileLogic.Create(filepath, html, Encoding.UTF8);
        }
    }
}
