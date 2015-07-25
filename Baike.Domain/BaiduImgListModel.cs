using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baike.Entity
{
    public class BaiduImgListModel
    {
        public string queryEnc { get; set; }
        public string queryExt { get; set; }
        public int listNum { get; set; }
        public int displayNum { get; set; }
        public string bdFmtDispNum { get; set; }
        public string bdSearchTime { get; set; }
        public string bdIsClustered { get; set; }

        public List<BaiduImgModel> data { get; set; }
    }

    public class BaiduImgModel
    {
        public string thumbURL { get; set; }
        public string middleURL { get; set; }
        public string largeTnImageUrl { get; set; }
        public int hasLarge { get; set; }
        public string hoverURL { get; set; }
        public int pageNum { get; set; }
        public string objURL { get; set; }
        public string fromURL { get; set; }
        public string fromURLHost { get; set; }
        public string currentIndex { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string type { get; set; }
        public string filesize { get; set; }
        public string bdSrcType { get; set; }
        public string di { get; set; }

        ///public string is{get;set;}0,0",
        public string adPicId { get; set; }
        public int bdSetImgNum { get; set; }
        public string bdImgnewsDate { get; set; }
        public string fromPageTitle { get; set; }
        public string fromPageTitleEnc { get; set; }
        public string bdSourceName { get; set; }
        public string bdFromPageTitlePrefix { get; set; }
        public string token { get; set; }
    }
}
