
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baike.Dataservice
{
    using System.Text.RegularExpressions;

    using Baike.Entity.DBModel;
    using Baike.Repository;

    using HQ.Common;

    public class BaiduTopService : Baseservice
    {
        private WebClientHelper webClient = new WebClientHelper();


        public BaiduTopService()
        {
            this.webClient.Encoding = Encoding.GetEncoding("GBK");
            unitOfWork = new UnitOfWork("wxmpConnString");
            this.siteid = siteid;
        }

        public void Main()
        {

            this.Toppage("http://top.baidu.com/buzz?b=17&c=9&fr=topbuzz", 1);
            this.Toppage("http://top.baidu.com/buzz?b=15&c=9&fr=topbuzz", 1);
            this.Toppage("http://top.baidu.com/buzz?b=22&c=9&fr=topbuzz", 1);

            this.Toppage("http://top.baidu.com/buzz?b=18&c=9&fr=topcategory", 2);
            this.Toppage("http://top.baidu.com/buzz?b=16&c=9&fr=topbuzz", 2);
            this.Toppage("http://top.baidu.com/buzz?b=3&c=9&fr=topbuzz", 2);
        }

        private void Toppage(string url, int categoryid)
        {
            var html = webClient.GetHtmlSource(url);
            if (!string.IsNullOrEmpty(html))
            {
                Regex regex = new Regex(@"<a\s*class=""list-title""[^<>]*>([^<>]*)</a>");
                MatchCollection ms = regex.Matches(html);
                foreach (Match m in ms)
                {
                    if (m.Success)
                    {
                        var name = m.Groups[1].Value;
                        if (!this.unitOfWork.MingxingRepository.Contains(c => c.Name == name))
                        {
                            this.unitOfWork.MingxingRepository.Insert(new Mingxing()
                                                                         {
                                                                             CategoryId = categoryid,
                                                                             Name = name
                                                                         });
                        }
                    }
                }
            }
        }
    }
}
