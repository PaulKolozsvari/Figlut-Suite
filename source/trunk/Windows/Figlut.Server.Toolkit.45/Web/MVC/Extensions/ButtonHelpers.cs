namespace Figlut.Server.Toolkit.Web.MVC.Extensions
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    #endregion //Using Directives

    //http://volaresystems.com/blog/post/2012/09/16/Making-spiffy-buttons-with-CSS-and-MVC-Razor-helpers
    public static class ButtonHelpers
    {
        public static MvcHtmlString LinkButtonForSubmit(
            this HtmlHelper helper,
            string buttonText,
            string javaScriptFunction,
            string controlId)
        {
            return LinkButtonForSubmit(helper, buttonText, javaScriptFunction, controlId, null, true, null);
        }

        public static MvcHtmlString LinkButtonForSubmit(
            this HtmlHelper helper,
            string buttonText,
            string javaScriptFunction,
            string controlId,
            string imageIconName)
        {
            return LinkButtonForSubmit(helper, buttonText, javaScriptFunction, controlId, null, true, imageIconName);
        }

        public static MvcHtmlString LinkButtonForSubmit(
            this HtmlHelper helper,
            string buttonText, 
            string javaScriptFunction,
            string controlId,
            string cssClass,
            bool includeImage)
        {
            return LinkButtonForSubmit(helper, buttonText, javaScriptFunction, controlId, cssClass, includeImage, null);
        }

        public static MvcHtmlString LinkButtonForSubmit(
            this HtmlHelper helper,
            string buttonText,
            string javaScriptFunction,
            string controlId,
            string cssClass,
            bool includeImage,
            string imageIconName)
        {
            string imageName = null;
            if (includeImage)
            {
                imageName = string.IsNullOrEmpty(imageIconName) ? "tick.png" : imageIconName;
            }
            TagBuilder imageTag = ImageTag(imageName);

            var anchorHtml = FormSubmitAnchorTag(imageTag, buttonText, javaScriptFunction, controlId, cssClass);
            MvcHtmlString result = MvcHtmlString.Create(anchorHtml);
            return result;
        }

        public static MvcHtmlString LinkButtonForCancel(
            this HtmlHelper helper, 
            string buttonText, 
            string onClickJavaScriptFunction,
            string controlId)
        {
            var areaName = AreaName(helper);
            string controllerName = ControllerName(helper);
            var imageTag = ImageTag("cross.png");
            var anchorHtml = AnchorTagWithImageAndText(
                areaName,
                controllerName,
                "Index",
                null,
                "button negative",
                imageTag,
                buttonText,
                onClickJavaScriptFunction,
                controlId);
            MvcHtmlString result = MvcHtmlString.Create(anchorHtml);
            return result;
        }

        private static TagBuilder ImageTag(string iconName)
        {
            var imageTag = new TagBuilder("img");
            if (iconName != null)
            {
                imageTag.MergeAttribute("src", VirtualPathUtility.ToAbsolute(string.Format("~/Images/Icons/{0}", iconName)));
            }
            imageTag.MergeAttribute("width", "16");
            imageTag.MergeAttribute("height", "16");
            imageTag.MergeAttribute("alt", "");
            return imageTag;
        }

        private static string AreaName(HtmlHelper helper)
        {
            var routeData = helper.ViewContext.RouteData;
            return routeData.DataTokens["area"] == null ? string.Empty : routeData.DataTokens["area"].ToString();
        }

        private static string ControllerName(HtmlHelper helper)
        {
            var routeData = helper.ViewContext.RouteData;
            return routeData.GetRequiredString("controller");
        }

        private static string FormSubmitAnchorTag(TagBuilder imageTag, string anchorText, string onClickJavaScriptFunction, string controlId, string cssClass)
        {
            var anchorTag = new TagBuilder("a");
            if (!string.IsNullOrEmpty(controlId))
            {
                anchorTag.MergeAttribute("id", controlId);
            }
            if (!string.IsNullOrEmpty(onClickJavaScriptFunction))
            {
                anchorTag.MergeAttribute("onclick", onClickJavaScriptFunction);
            }
            else
            {
                anchorTag.MergeAttribute("href", "#");
            }
            if (cssClass == null)
            {
                anchorTag.AddCssClass("button positive");
            }
            else
            {
                anchorTag.AddCssClass(cssClass);
            }
            anchorTag.InnerHtml = imageTag.ToString(TagRenderMode.SelfClosing) + anchorText;
            string result = anchorTag.ToString();
            return result;
        }

        private static string AnchorTagWithImageAndText(
            string areaName, 
            string controllerName, 
            string actionName, int? id, 
            string cssClass, 
            TagBuilder imageTag, 
            string anchorText,
            string onClickJavaScriptFunction,
            string controlId)
        {
            var anchorTag = new TagBuilder("a");
            if (!string.IsNullOrEmpty(controlId))
            {
                anchorTag.MergeAttribute("id", controlId);
            }
            if (!string.IsNullOrEmpty(onClickJavaScriptFunction))
            {
                anchorTag.MergeAttribute("onclick", onClickJavaScriptFunction);
            }
            else
            {
                if (areaName != string.Empty)
                {
                    anchorTag.MergeAttribute("href", id == null
                                                         ? string.Format("/{0}/{1}/{2}", areaName, controllerName, actionName)
                                                         : string.Format("/{0}/{1}/{2}/{3}", areaName, controllerName, actionName, id));
                }
                else
                {
                    anchorTag.MergeAttribute("href", id == null
                                                         ? string.Format("/{0}/{1}", controllerName, actionName)
                                                         : string.Format("/{0}/{1}/{2}", controllerName, actionName, id));
                }
            }
            anchorTag.AddCssClass(cssClass);
            anchorTag.InnerHtml = imageTag.ToString(TagRenderMode.SelfClosing) + anchorText;
            return anchorTag.ToString();
        }
    }
}