using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baike.Dataservice
{
    using System.IO;
    using System.Web;
    using System.Web.Script.Serialization;

    using Baike.Entity;
    using Baike.Entity.DBModel;
    using Baike.Repository;

    using HQ.Common;

    public class ImgService : Baseservice
    {
        /// <summary>
        /// webclient
        /// </summary>
        private WebClientHelper webClient = new WebClientHelper();

        private WebSite webSite = null;

        private string baiduimgurl =
            "http://image.baidu.com/i?ct=201326592&cl=2&lm=-1&st=-1&tn=baiduimagejson&istype=2&rn=32&fm=index&pv=&word={0}&s=1&z=19&1412319676243&callback=bd__cbs__742spx";

        private JavaScriptSerializer javaScriptSerializer;

        public ImgService(int siteid)
        {
            this.javaScriptSerializer = new JavaScriptSerializer();
            webClient.Encoding = Encoding.GetEncoding("GBK");
            unitOfWork = new UnitOfWork("ConnString");
            this.siteid = siteid;
            this.webSite = this.unitOfWork.WebSiteRepository.Find(c => c.Id == siteid);
        }


        public void Main()
        {
            List<string> keywords = new List<string>();
            var nodes = this.unitOfWork.NodeRepository.GetAll();
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    if (!string.IsNullOrEmpty(node.Keywords))
                    {
                        var keywrodss = node.Keywords.Replace("&", string.Empty).Split(',');
                        keywords.AddRange(keywrodss.ToList());
                    }
                }

                foreach (var k in keywords)
                {
                    try
                    {

                        var keyword = k.Trim();
                        var htmljson =
                            this.webClient.GetHtmlSource(string.Format(baiduimgurl, HttpUtility.UrlEncode(keyword, Encoding.GetEncoding("GBK"))));
                        htmljson = htmljson.Replace("bd__cbs__742spx(", string.Empty);
                        htmljson = htmljson.Replace(")", string.Empty);
                        var imgmodel = this.javaScriptSerializer.Deserialize<BaiduImgListModel>(htmljson);

                        if (imgmodel != null && imgmodel.data != null)
                        {
                            int i = 0;
                            foreach (var img in imgmodel.data)
                            {
                                if (i >= 20)
                                {
                                    break;
                                }

                                if (!this.unitOfWork.KeywordImgRepository.Contains(c => c.Title == img.fromPageTitleEnc))
                                {
                                    string newimgurl = ImageUploadLogic.DownloadImg(
                                        img.objURL,
                                        this.webSite.PhysicalPath,
                                        this.webSite.DomainName);
                                    if (!string.IsNullOrEmpty(newimgurl))
                                    {
                                        var title = !string.IsNullOrEmpty(img.fromPageTitleEnc)
                                                    && img.fromPageTitleEnc.Length > 25
                                                        ? img.fromPageTitleEnc.Substring(0, 24)
                                                        : img.fromPageTitleEnc;
                                        this.unitOfWork.KeywordImgRepository.Insert(
                                            new KeywordImg()
                                                {
                                                    Keyword = keyword,
                                                    ImgUrl = newimgurl,
                                                    SiteId = this.siteid,
                                                    Title = title,
                                                    Height = img.height,
                                                    Width = img.width,
                                                    thumbURL = img.thumbURL
                                                });
                                    }
                                }

                                i++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                      Log.service.Logger.Error(ex);
                    }
                }
            }
        }
    }
}
