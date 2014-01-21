namespace Expenses.Web.Extensions
{
    using System;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public static class HtmlExtensions
    {
        public static MvcHtmlString MenuActionLink(
            this HtmlHelper htmlHelper,
            string linkText,
            string action,
            string controller,
            object routeValues = null)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            var classValues = linkText.ToLowerInvariant().Replace(" ", string.Empty);

            if (action.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase) &&
                controller.Equals(currentController, StringComparison.InvariantCultureIgnoreCase))
            {
                var request = htmlHelper.ViewContext.RequestContext.HttpContext.Request;
                if (request["status"] == null && routeValues == null)
                {
                    classValues += " selected";
                }
                else if (request["status"] != null && routeValues != null)
                {
                    Type t = routeValues.GetType();
                    PropertyInfo p = t.GetProperty("status");
                    var status = (string)p.GetValue(routeValues, null);

                    if (request["status"].Equals(status, StringComparison.InvariantCultureIgnoreCase))
                    {
                        classValues += " selected";
                    }
                }
            }

            return htmlHelper.ActionLink(linkText, action, controller, routeValues, new { @class = classValues });
        }
    }
}