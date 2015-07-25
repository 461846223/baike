using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baike.Dataservice
{
    using Baike.Repository;

    public class Baseservice
    {

        /// <summary>
        /// 站点
        /// </summary>
        protected int siteid { get; set; }

        /// <summary>
        /// The unit of work.
        /// </summary>
        protected UnitOfWork unitOfWork;
        protected List<NodeKeyword> InitializationNodeKeyword()
        {
            var r = new List<NodeKeyword>();
            ////初始化关键字和目录关系
            var nodes = this.unitOfWork.NodeRepository.Get(c => c.SiteId == siteid);
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    if (!string.IsNullOrEmpty(node.Keywords))
                    {
                        var keywords = node.Keywords.Split(',');
                        if (keywords.Length > 0)
                        {
                            foreach (var keyword in keywords)
                            {
                                if (string.IsNullOrEmpty(keyword)) continue;

                                var childkeys = keyword.Split('&');

                                r.Add(
                                    new NodeKeyword() { Andkeywords = childkeys.ToList(), Nodeid = node.Id });
                            }
                        }
                    }
                }

                r = r.OrderByDescending(c => c.Andkeywords.Count()).ToList();
            }

            return r;
        }
    }
}
