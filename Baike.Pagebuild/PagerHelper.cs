using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Baike.Pagebuild
{
    using HQ.Common.Paging;

    /// <summary>
    /// PagerHelper
    /// </summary>
    public static class PagerHelper
    {
        /// <summary>
        /// Pager
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="pagedable"></param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, IPagedable pagedable)
        {
            return RenderPager(helper, pagedable);
        }

        /// <summary>
        /// Pager
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="pagedable"></param>
        /// <param name="pagingTemplatePartialName"></param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, IPagedable pagedable, string pagingTemplatePartialName)
        {
            return RenderPager(helper, pagedable, pagingTemplatePartialName);
        }

        /// <summary>
        /// 生成最终的分页Html代码
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="pagedable"></param>
        /// <returns></returns>
        private static MvcHtmlString RenderPager(HtmlHelper htmlHelper, IPagedable pagedable)
        {
            return RenderPager(htmlHelper, pagedable, "PagingTemplate");
        }

        /// <summary>
        /// 生成最终的分页Html代码
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="pagedable"></param>
        /// <param name="pagingTemplatePartialName"></param>
        /// <returns></returns>
        private static MvcHtmlString RenderPager(HtmlHelper htmlHelper, IPagedable pagedable, string pagingTemplatePartialName)
        {
            if (pagedable.PageNumber <= 0)
            {
                pagedable.PageNumber = 1;
            }

            if (pagedable.PageNumber > 0 && pagedable.PageNumber >= pagedable.PageCount)
            {
                pagedable.PageNumber = pagedable.PageCount;
            }

            var templateHtml = htmlHelper.Partial(pagingTemplatePartialName, pagedable);

            if (templateHtml == null)
                throw new ArgumentException(pagingTemplatePartialName);
            return templateHtml;
        }
    }
}