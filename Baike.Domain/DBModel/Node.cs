using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace Baike.Entity{
    using System.Collections.ObjectModel;

    //Node
    public class Node : BaseEntity
    {
        public Node()
        {
        }

        public string Title { get; set; }

        public int ParentId { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Content> Contents { get; set; }

        public int SiteId { get; set; }
    }
}
