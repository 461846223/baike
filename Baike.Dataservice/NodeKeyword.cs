using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baike.Dataservice
{
    /// <summary>
    /// Node的Keyword 例如：微信&营销&案例,微信&案例
    /// </summary>
    public class NodeKeyword
    {
        /// <summary>
        /// &&关系的关键字例如：微信&营销&案例
        /// </summary>
        public List<string> Andkeywords { get; set; }

        /// <summary>
        /// nodeid
        /// </summary>
        public int Nodeid { get; set; }
    }
}
