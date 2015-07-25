using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baike.Pagebuild.Models
{
    using Baike.Entity;

    public class HomePageModel : BaseModel
    {
        public HomePageModel()
        {
            this.NodeModels = new List<NodeModel>();
        }

        /// <summary>
        /// 内容分类列表
        /// </summary>
        public List<NodeModel> NodeModels { get; set; } 
    }

    public class NodeModel
    {
        public NodeModel()
        {
            this.Items = new List<Content>();
            this.NodeInfo = new Node();
        }

        /// <summary>
        /// 分类目录
        /// </summary>
        public Node NodeInfo { get; set; }

        /// <summary>
        /// 文章列表页
        /// </summary>
        public List<Content> Items { get; set; }
    }
}
