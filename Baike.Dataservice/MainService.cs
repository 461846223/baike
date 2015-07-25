using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baike.Dataservice
{
    using System.Net.NetworkInformation;
    using System.Security.Cryptography.X509Certificates;
    using System.Text.RegularExpressions;

    using Baike.Entity;
    using Baike.Entity.DBModel;
    using Baike.Repository;

    using HQ.Common;

    using NClassifier.Summarizer;

    using StanSoft;

    public class MainService : Baseservice
    {


        private SimpleSummarizer summarizer = new SimpleSummarizer();

        /// <summary>
        /// webclient
        /// </summary>
        private WebClientHelper webClient = new WebClientHelper();

        private Regex regexa = new Regex(@"(?is)<a[^>]*?href=(['""]?)(?<url>[^'""\s>]+)\1[^>]*>(?<text>(?:(?!</?a\b).)*)</a>");

        /// <summary>
        /// 关键字和目录对照
        /// </summary>
        private List<NodeKeyword> keywordDictionary = new List<NodeKeyword>();



        /// <summary>
        /// 构造函数
        /// </summary>
        public MainService(int siteid)
        {
            this.unitOfWork = new UnitOfWork();
            this.siteid = siteid;
            this.keywordDictionary = this.InitializationNodeKeyword();
        }

        /// <summary>
        /// Site
        /// </summary>
        public void Site()
        {
            ////获取站点信息
            var webSite = this.unitOfWork.WebSiteRepository.Find(siteid);
            if (webSite != null)
            {
                /////遍历站点数据源
                foreach (var ds in webSite.DataSouces)
                {
                    var secondlevelurls = this.ContentPre(ds, false);

                    if (secondlevelurls != null && secondlevelurls.Count > 0)
                    {
                        foreach (var secondds in secondlevelurls)
                        {
                            this.ContentPre(secondds, false);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 检查文章源url和和标题
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <param name="isfirstlevelurl">是否第一级人口</param>
        private List<DataSource> ContentPre(DataSource ds, bool isfirstlevelurl)
        {
            var r = new List<DataSource>();
            try
            {


                ////数据源文章页面url正则表达式
                string regexpar = string.Format(@"{0}", ds.ContentRegEx);
                if (string.IsNullOrEmpty(ds.ContentRegEx))
                {
                    regexpar = string.Format(@"{0}", Regexpattern.regexA);
                }

                var contentRegex = new Regex(regexpar);
                this.webClient.Encoding = Encoding.GetEncoding(ds.Encode);

                if (!Uri.IsWellFormedUriString(ds.IntoUrl, UriKind.Absolute) && !this.CheckUrl(ds.IntoUrl))
                {
                    return r;
                }



                var intohtml = this.webClient.GetHtmlSource(ds.IntoUrl);

                if (!string.IsNullOrEmpty(intohtml))
                {
                    ////获取url列表
                    MatchCollection matchs = contentRegex.Matches(intohtml);

                    foreach (Match match in matchs)
                    {
                        if (match.Success)
                        {

                            if (match.Groups[1].Value.IndexOf("javascript:") > -1)
                            {
                                continue;
                            }

                            if (string.IsNullOrEmpty(match.Groups[1].Value))
                            {
                                continue;
                            }


                            var contenturl = UrlHelper.CompleteURL(ds.IntoUrl, match.Groups[1].Value.Trim());

                            if (string.IsNullOrEmpty(contenturl))
                            {
                                continue;
                            }

                            if (!UrlHelper.IsSameDomain(ds.IntoUrl, contenturl))
                            {
                                continue;
                            }

                            var title = match.Groups[2].Value;

                            if (string.IsNullOrEmpty(title))
                            {
                                continue;
                            }

                            title = this.FilterText(title, ds.RemovewordsList);

                            ////检查是否是包含需要的关键字
                            var nodeid = this.CheckKeyword(title);
                            if (nodeid == 0)
                            {
                                if (isfirstlevelurl)
                                {
                                    r.Add(new DataSource()
                                              {
                                                  IntoUrl = contenturl,
                                                  Removewords = ds.Removewords,
                                                  Encode = ds.Encode,
                                                  ContentRegEx = ds.ContentRegEx,
                                                  WebsiteId = ds.WebsiteId
                                              });
                                }

                                continue;
                            }

                            /////根据标题判断库里是否是存在相同的标题
                            var iscontent =
                                this.unitOfWork.ContentRepository.Find(c => c.Title.ToLower() == title.ToLower());

                            if (iscontent != null && iscontent.Id > 0)
                            {
                                continue;
                            }

                            ////提取文章并入库
                            this.Content(contenturl, ds.RemovewordsList);
                        }
                    }
                }


            }
            catch (Exception ex)
            {

            }

            return r;
        }

        /// <summary>
        /// 内容处理
        /// </summary>
        /// <param name="contenturl"></param>
        private void Content(string contenturl, List<string> removewords)
        {
            try
            {

                if (!string.IsNullOrEmpty(contenturl))
                {
                    contenturl = contenturl.Trim();
                    ////判断是否采集过
                    var history = this.unitOfWork.DataUrlHistoryRepository.Find(c => c.Url.ToLower() == contenturl);
                    if (history != null && history.Id > 0)
                    {
                        return;
                    }

                    ////获取源码
                    var htmltext = string.Empty;

                    try
                    {
                        htmltext = this.webClient.GetHtmlSource(contenturl);
                    }
                    catch (Exception ex)
                    {
                    }


                    if (!string.IsNullOrEmpty(htmltext))
                    {
                        ////获取文章
                        var article = this.GetArticle(htmltext, removewords);

                        if (article != null)
                        {
                            if (!string.IsNullOrEmpty(article.Title) && !string.IsNullOrEmpty(article.Content))
                            {
                                ////检查是否是包含需要的关键字
                                var nodeid = this.CheckKeyword(article.Title);
                                if (nodeid == 0)
                                {
                                    return;
                                }

                                /////根据标题判断库里是否是存在相同的标题
                                var iscontent =
                                    this.unitOfWork.ContentRepository.Find(
                                        c => c.Title.ToLower() == article.Title.ToLower());

                                if (iscontent != null && iscontent.Id > 0)
                                {
                                    return;
                                }

                                ////简介
                                var summary = this.summarizer.Summarize(article.Content, 1);

                                if (summary != null && summary.Length > 250)
                                {
                                    summary = summary.Substring(0, 250);
                                }

                                if (article.Title.Length > 50)
                                {
                                    summary = summary.Substring(0, 50);
                                }

                                ////上一篇文章
                                int lastid = this.GetPreviousId(nodeid);

                                var imgs = HtmlHelper.GetImgsUrl(article.ContentWithTags, contenturl);



                                var defaultimg = string.Empty;
                                if (imgs.Any())
                                {
                                    foreach (var img in imgs)
                                    {
                                        if (string.IsNullOrEmpty(img) || img.Length > 100)
                                        {
                                            continue;
                                        }

                                        defaultimg = img;
                                        break;
                                    }
                                }


                                article.ContentWithTags = this.CompletecontentimgURL(article.ContentWithTags, contenturl);


                                try
                                {

                                    var keyword = this.GetKeyword(article.Title);

                                    ////添加数据
                                    var newcontent =
                                        this.unitOfWork.ContentRepository.Insert(
                                            new Content()
                                                {
                                                    Title = article.Title,
                                                    ContentText = article.ContentWithTags,
                                                    NodeId = nodeid,
                                                    Summary = summary,
                                                    PreviousId = lastid,
                                                    ImageUrl = defaultimg,
                                                    SEOkeywords = keyword,
                                                });
                                    if (newcontent != null && newcontent.Id > 0)
                                    {
                                        this.unitOfWork.DataUrlHistoryRepository.Insert(
                                            new DataUrlHistory() { Url = contenturl, InserTime = DateTime.Now });
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private string CompletecontentimgURL(string p, string contenturl)
        {
            string result = string.Copy(p);
            if (!string.IsNullOrEmpty(result))
            {
                var regex = new Regex(@"src=""([^""]*)""");
                MatchCollection matchs = regex.Matches(result);

                ////src="/upload/html/20140928121143317.jpg"

                if (matchs.Count > 0)
                {
                    foreach (Match m in matchs)
                    {
                        var src = m.Groups[1].Value;
                        var completesrc = UrlHelper.CompleteURL(contenturl, src);
                        result = result.Replace(src, completesrc);
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// 检查标题内的关键字，并根据关键字找到指定的目录id
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        private int CheckKeyword(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return 0;
            }

            foreach (var item in keywordDictionary)
            {
                if (item.Andkeywords == null || item.Andkeywords.Count == 0)
                {
                    continue;
                }

                bool istrue = false;

                istrue = item.Andkeywords.All(title.ToLower().Contains);

                if (istrue)
                {
                    return item.Nodeid;
                }
            }

            return 0;
        }


        private string GetKeyword(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return string.Empty;
            }

            foreach (var item in keywordDictionary)
            {
                if (item.Andkeywords.All(title.ToLower().Contains))
                {
                    return string.Join("", item.Andkeywords);

                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取同一目录下前一个id
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        private int GetPreviousId(int nodeid)
        {
            int ouncount = 0;
            int lastid = 0;
            var lastitems = this.unitOfWork.ContentRepository.Get(
                c => c.NodeId == nodeid,
                out ouncount,
                0,
                1,
                o => o.OrderByDescending(c => c.Id));
            if (lastitems != null && lastitems.Any())
            {
                lastid = lastitems.ToList()[0].Id;
            }

            return lastid;
        }

        /// <summary>
        /// 过滤内容
        /// </summary>
        /// <param name="text">被过滤的内容</param>
        /// <param name="removewords">关键词</param>
        /// <returns></returns>
        private string FilterText(string text, List<string> removewords)
        {
            var r = string.Copy(text);
            if (!string.IsNullOrEmpty(text) && removewords != null)
            {
                foreach (var word in removewords)
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        r = r.Replace(word, string.Empty);
                    }
                }
            }

            return r;
        }

        /// <summary>
        /// 提取文章标题和内容
        /// </summary>
        /// <param name="htmltext">
        /// The htmltext.
        /// </param>
        /// <param name="removewords">
        /// The removewords.
        /// </param>
        /// <returns>
        /// The <see cref="Article"/>.
        /// </returns>
        private Article GetArticle(string htmltext, List<string> removewords)
        {
            ////获取文章
            Html2Article.AppendMode = true;
            var article = Html2Article.GetArticle(htmltext);

            if (article != null)
            {
                if (!string.IsNullOrEmpty(article.Title) && !string.IsNullOrEmpty(article.Content))
                {

                    article.Title = this.FilterText(article.Title, removewords);
                    article.Content = this.FilterText(article.Content, removewords);
                    article.ContentWithTags = this.FilterText(article.ContentWithTags, removewords);


                    MatchCollection matchs = this.regexa.Matches(article.ContentWithTags);
                    foreach (Match match in matchs)
                    {
                        article.ContentWithTags = article.ContentWithTags.Replace(
                            match.Groups[0].Value,
                            match.Groups[match.Groups.Count - 1].Value);
                        article.Content = article.Content.Replace(
                           match.Groups[0].Value,
                           match.Groups[match.Groups.Count - 1].Value);
                    }
                }
            }

            return article;
        }


        public bool CheckUrl(string str)
        {
            var RegUrl = new Regex(@"^[A-Za-z]+://[A-Za-z0-9-_]+\.[A-Za-z0-9-_%&?/.=]+$");

            if (!RegUrl.Match(str).Success)
            {
                return false;
            }
            return true;
        }
    }
}