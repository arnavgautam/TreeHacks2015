namespace BuildClips.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    
    public static class MvcExtensions
    {
        public static MvcHtmlString ActionActiveLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            var htmlAttributes = new RouteValueDictionary();
            var currentControllerName = htmlHelper.ViewContext.RouteData.Values["controller"].ToString();
            var currentActionName = htmlHelper.ViewContext.RouteData.Values["action"].ToString();

            if (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase) &&
                currentActionName.Equals(actionName, StringComparison.CurrentCultureIgnoreCase))
            {
                htmlAttributes.Add("class", "active");
            }

            return htmlHelper.ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(), htmlAttributes);
        }
    }
}
