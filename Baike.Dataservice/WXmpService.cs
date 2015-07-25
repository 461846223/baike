using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baike.Dataservice
{
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web.UI;

    using Baike.Entity;
    using Baike.Repository;

    using HQ.Common;

    using Log.service;

    using Microsoft.SqlServer.Server;

    public class WXmpService:Baseservice
    {
        /// <summary>
        /// The filebasepath.
        /// </summary>
        private string filebasepath = @"D:\website\dq.hiweixin.com.cn";

        /// <summary>
        /// 关键字和目录对照
        /// </summary>
        private List<NodeKeyword> keywordDictionary = new List<NodeKeyword>();

        /// <summary>
        /// webclient
        /// </summary>
        private WebClientHelper webClient = new WebClientHelper();


        public WXmpService(int siteid)
        {
            webClient.Encoding = Encoding.UTF8;
            unitOfWork = new UnitOfWork("ConnString");
            this.siteid = siteid;
            this.keywordDictionary = this.InitializationNodeKeyword();
        }

        public void Main()
        {
            foreach (var node in keywordDictionary)
            {
                foreach (var k in node.Andkeywords)
                {
                  
                    this.wxmllist(string.Format("http://weixin.sogou.com/weixin?type=1&query={0}&ie=utf8&_ast=1411196288&_asf=null&w=01029901&p=40040100&dp=1&cid=null", k), node.Nodeid, k);
                    Thread.Sleep(10000);
                }
            }
        }

        /// <summary>
        /// 明星微信
        /// </summary>
        public void Mingxing()
        {
          var mingxings=   unitOfWork.MingxingRepository.GetAll().ToList();
            foreach (var mingxing in mingxings)
            {
                var cid = 0;
                switch (mingxing.CategoryId)
                {
                    case 1:
                        cid = 12;
                        break;
                    case 2:
                        cid = 13;
                        break;
                }

                if (cid > 0)
                {
                    this.wxmllist(
                        string.Format(
                            "http://weixin.sogou.com/weixin?type=1&query={0}&ie=utf8&_ast=1411196288&_asf=null&w=01029901&p=40040100&dp=1&cid=null",
                            mingxing.Name),
                        cid,
                        mingxing.Name);
                    Thread.Sleep(10000);
                }
            }
        }


        private void wxmllist(string url, int nodeid,string k)
        {
             var referer ="http://weixin.sogou.com/?p=73141200&kw=";

            var html = this.webClient.GetHtmlSource(url,referer);

           var isjixue = this.Getwxmpinfo2DB(html, nodeid,k);

            if (!isjixue)
            {
                return;
            }
            ////获取总条数
            var regexnum = new Regex(@"<resnum id=""scd_num"">([^<>]*)</resnum>");

            var matchnum = regexnum.Match(html);
            if (matchnum.Success && matchnum.Groups.Count > 1)
            {
                int intnum = 0;
                var num = matchnum.Groups[1].Value;
                num = num.Replace(",", string.Empty);
                int.TryParse(num, out intnum);
                var pages = (int)Math.Ceiling((Double)intnum / 10);

                if (pages > 1)
                {
                    for (int i = 2; i <= pages; i++)
                    {
                        referer = string.Format("{0}&page={1}", url, i - 1);

                        try
                        {



                            html = this.webClient.GetHtmlSource(string.Format("{0}&page={1}", url, i), referer);
                            isjixue = this.Getwxmpinfo2DB(html, nodeid, k);

                            if (!isjixue)
                            {
                                return;
                            }

                        }
                        catch (Exception ex)
                        {
                        }

                        Thread.Sleep(6000);
                        ////

                    }
                }
            }

            ////总条数：<resnum id="scd_num">39,610</resnum>
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        private bool Getwxmpinfo2DB(string html, int nodeid,string k)
        {
            ////是否继续往下遍历
            bool r = true;
            ////列表
            var regex = new Regex(string.Format(Regexpattern.regexDiv, " class=\"wx-rb bg-blue wx-rb_v1 _item\"[^<>]*"));
            MatchCollection ms = regex.Matches(html);

            if (ms.Count == 0)
            {
                Logger.Info("Getwxmpinfo2DB ms.Count == 0");
            }

            foreach (Match m in ms)
            {
                var cinfo = new Content();
                try
                {
                   
                    cinfo.NodeId = nodeid;

                    regex = new Regex("<span>微信号：([^<>]*)</span>");

                    Match match = regex.Match(m.Value);

                    if (match.Success && match.Groups.Count > 0)
                    {
                        cinfo.Subtitle = match.Groups[1].Value;
                    }

                    if (this.unitOfWork.ContentRepository.Contains(c => c.Subtitle == cinfo.Subtitle))
                    {
                        continue;
                    }


                    regex = new Regex(@"<h3>([\s\S]*?)</h3>");
                    match = regex.Match(m.Value);
                    if (match.Success && match.Groups.Count > 0)
                    {
                        cinfo.Title = match.Groups[1].Value.Replace("<!--red_beg-->", string.Empty);
                        cinfo.Title = cinfo.Title.Replace("<!--red_end-->", string.Empty);
                        cinfo.Title = cinfo.Title.Replace("</em>", string.Empty);
                        cinfo.Title = cinfo.Title.Replace("<em>", string.Empty);

                        if (nodeid == 14 || nodeid == 13)
                        {
                            if (cinfo.Title != k)
                            {
                                r = false;
                                return r;
                            }
                        }

                        if (!cinfo.Title.Contains(k))
                        {
                            r = false;
                            return r;
                        }
                    }

                    regex = new Regex("功能介绍：</span>" + string.Format(Regexpattern.regexspan, " class=\"sp-txt\""));
                    match = regex.Match(m.Value);
                    if (match.Success)
                    {
                        cinfo.ContentText = string.Format(
                            "<p>{0}</p>",
                            match.Value.Replace("功能介绍：</span>", string.Empty));
                    }

                    regex = new Regex("认证：</span>" + string.Format(Regexpattern.regexspan, " class=\"sp-txt\""));
                    match = regex.Match(m.Value);
                    if (match.Success)
                    {
                        cinfo.ContentText = cinfo.ContentText
                                            + string.Format(
                                                "<p><span>认证：</span>{0}</p>",
                                                match.Value.Replace("认证：</span>", string.Empty));
                    }


                    regex = new Regex(@"url=([^""]*)'");

                    match = regex.Match(m.Value);
                    if (match.Success)
                    {
                        cinfo.ImageUrl = imgdownload(
                            "http://img01.sogoucdn.com",
                            match.Groups[1].Value,
                            "http://dq.hiweixin.com.cn",
                            this.filebasepath);
                    }

                    regex = new Regex(Regexpattern.regeximgsrc);

                    MatchCollection matchs = regex.Matches(m.Value);

                    if (matchs.Count >= 3)
                    {

                        cinfo.ImageUrlse = imgdownload(
                            "http://img01.sogoucdn.com",
                            matchs[2].Groups[1].Value,
                            "http://dq.hiweixin.com.cn",
                            this.filebasepath);
                    }


                    cinfo.Summary = HtmlHelper.NoHTML(cinfo.ContentText);
                    if (cinfo.Summary != null && cinfo.Summary.Length > 250)
                    {
                        cinfo.Summary = cinfo.Summary.Substring(0, 250);
                    }

                    cinfo.SEOkeywords = string.Format("{0}微信号", cinfo.Title);
                    cinfo.SEOdescription = string.Format(
                        "{0}微信号:{1}",
                        cinfo.Title,
                        HQ.Common.HtmlHelper.NoHTML(cinfo.ContentText));


                    if (cinfo.SEOdescription != null && cinfo.SEOdescription.Length > 100)
                    {
                        cinfo.SEOdescription = cinfo.SEOdescription.Substring(0, 100);
                    }

                    this.unitOfWork.ContentRepository.Insert(cinfo);

                }
                catch (Exception ex)
                {
                }
            }

            return r;
        }


        private string imgdownload(string mediamasterurl, string imgsrc, string newurlbase, string filebasepath)
        {
            var completesrc = UrlHelper.CompleteURL(mediamasterurl, imgsrc);

            string datepath = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "\\");

            TimeSpan ts = (DateTime.Now - DateTime.Today);

            var srcuri = new Uri(completesrc);
            var filename = srcuri.Segments.Last();
            if (filename == null || filename.IndexOf(".", System.StringComparison.Ordinal) < 0)
            {
                filename = string.Format("{0}.jpg", ts.Ticks);
            }
            else
            {
                var fileInfo = new FileInfo(filename);
                filename = string.Format("{0}{1}", ts.Ticks, fileInfo.Extension);
            }

            webClient.DownloadFile(completesrc, string.Format("{0}\\imgs\\{1}\\{2}", filebasepath, datepath, filename));

            var newurl = string.Format("{0}/imgs/{1}/{2}", newurlbase, datepath.Replace("\\", "/"), filename);

            return newurl;
        }
    }
}
