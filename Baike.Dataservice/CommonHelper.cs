using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Baike.Entity;
using HQ.Common;

namespace Baike.Dataservice
{
    public class CommonHelper
    {
        private static WebClientHelper webClient = new WebClientHelper();
        public static string Removeurl(string content)
        {
            string r = string.Copy(content);
            if (!string.IsNullOrEmpty(content))
            {
                var regex = new Regex(Regexpattern.regexA3);
                MatchCollection matchs = regex.Matches(content);

                if (matchs.Count > 0)
                {
                    r = matchs.Cast<Match>()
                        .Aggregate(r, (current, match) => r.Replace(match.Value, match.Groups[2].Value));
                }

                regex = new Regex(@"<a[^<>]*href=""([^""]*)""[^<>]*>()");
                r = regex.Replace(r, string.Empty);
                r = r.Replace("</a>", string.Empty);
                return r;
            }

            return content;
        }

        public static string Replacewords(string content, List<string> words)
        {
            string r = string.Copy(content);
            if (!string.IsNullOrEmpty(content))
            {
                r = words.Aggregate(r, (current, word) => current.Replace(word, string.Empty));
            }

            return r;
        }

        public static string Replacebyregex(string content, List<string> regexpatterns)
        {
            string r = string.Copy(content);
            Regex regex = null;
            foreach (var regexp in regexpatterns)
            {
                regex = new Regex(regexp);
                r = regex.Replace(r, string.Empty);
            }

            return r;
        }

        public static ContentTextandImg ImageDownloadandReplace(string content, string masterurl, string filebasepath, string newurlbase)
        {
            try
            {

                var r = new ContentTextandImg();
                string tempcontent = string.Copy(content);
                if (!string.IsNullOrEmpty(content))
                {
                    var regex = new Regex(Regexpattern.regeximg);

                    MatchCollection matchs = regex.Matches(tempcontent);
                    var srclist = new List<string>();

                    int i = 0;
                    foreach (Match m in matchs)
                    {
                        if (m.Success)
                        {
                            i++;
                            if (i == 1 || i == matchs.Count)
                            {
                                tempcontent = tempcontent.Replace(m.Value, string.Empty);
                                continue;
                            }

                            var src = m.Groups[1].Value;

                            if (srclist.Contains(src))
                            {
                                continue;
                            }

                            var completesrc = UrlHelper.CompleteURL(masterurl, src);

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

                            webClient.DownloadFile(completesrc, string.Format("{0}\\{1}\\{2}", filebasepath, datepath, filename));

                            var newurl = string.Format("{0}/{1}/{2}", newurlbase, datepath.Replace("\\", "/"), filename);

                            tempcontent = tempcontent.Replace(src, newurl);

                            /////记录处理过的图片避免重复下载
                            srclist.Add(src);
                            if (string.IsNullOrEmpty(r.ImageUrl))
                            {
                                r.ImageUrl = newurl;
                            }


                        }
                    }
                    r.ContentText = tempcontent;
                }

                return r;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static string ReplaceScript(string content)
        {
            string r = string.Copy(content);
            if (!string.IsNullOrEmpty(content))
            {
                Regex regex = new Regex(Regexpattern.regexscript);

                r = regex.Replace(r, string.Empty);
            }

            return r;
        }


        internal static object ImageDownloadand(string imgsrc, string p1, string p2)
        {
            throw new NotImplementedException();
        }
    }
}
