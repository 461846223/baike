using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baike.Pagebuild.Models
{
    public class ListRequest
    {
        public ListRequest()
        {
            this.Pagesize = 20;
            this.Pageindex = 1;
        }

        /// <summary>
        /// 节点id
        /// </summary>
        public int Nodeid { get; set; }

        /// <summary>
        /// Pagesize
        /// </summary>
        public int Pagesize { get; set; }

        /// <summary>
        /// Pageindex
        /// </summary>
        public int Pageindex { get; set; }
    }
}
