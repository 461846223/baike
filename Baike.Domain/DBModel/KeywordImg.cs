using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baike.Entity.DBModel
{
    public class KeywordImg : BaseEntity
    {
        public string Keyword { get; set; }
        public string Title { get; set; }
        public string ImgUrl { get; set; }
        public int SiteId { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string thumbURL { get; set; }
    }
}
