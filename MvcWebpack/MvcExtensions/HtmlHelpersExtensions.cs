using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcWebpack.MvcExtensions
{
    public static class HtmlHelpersExtensions
    {
        public static IHtmlString WebPackScript(this HtmlHelper helper, string bundleName, bool toAbsoluteUrl = true)
        {
            var jsTagBuilder = new TagBuilder("script");
            if (ConfigurationManager.AppSettings["WebpackMode"].ToLower() == "local")
            {
                string port = ConfigurationManager.AppSettings["WebpackPort"] ?? "3000";
                jsTagBuilder.Attributes.Add("src", $"http://localhost:{port}/static/js/{bundleName}-bundle.js");
                TagBuilder tbErrors = new TagBuilder("script");
                tbErrors.Attributes.Add("src", $"http://localhost:{port}/static/js/errors-bundle.js");
                return new HtmlString(jsTagBuilder.ToString());
            }
            string path = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~/Content/reactApp/build/static/js/{bundleName}-bundle.js");
            string queryParameter = $"?noBundleCache={DateTime.Now.Ticks}";
            jsTagBuilder.Attributes.Add("src", toAbsoluteUrl ? $"{new Uri(HttpContext.Current.Request.Url, path)}{queryParameter}" : $"{path}{queryParameter}");
            return HttpContext.Current.Request.IsSecureConnection
                ? new HtmlString(jsTagBuilder.ToString())
                : new HtmlString(jsTagBuilder.ToString().Replace("http", "https"));
        }

        public static IHtmlString WebPackCss(this HtmlHelper helper, string appName, bool toAbsoluteUrl = true)
        {
            var cssTagBuilder = new TagBuilder("link");
            cssTagBuilder.Attributes.Add("rel", "stylesheet");
            cssTagBuilder.Attributes.Add("stylesheet", "text/css");
            if (ConfigurationManager.AppSettings["WebpackMode"].ToLower() == "local")
            {
                string port = ConfigurationManager.AppSettings["WebpackPort"] ?? "3000";
                cssTagBuilder.Attributes.Add("href", $"http://localhost:{port}/static/css/{appName}.css");
                return new HtmlString(cssTagBuilder.ToString());
            }
            string path = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~/Content/reactApp/build/static/css/{appName}.css");
            string queryParameter = $"?noBundleCache={DateTime.Now.Ticks}";
            cssTagBuilder.Attributes.Add("href", toAbsoluteUrl ? $"{new Uri(HttpContext.Current.Request.Url, path)}{queryParameter}" : $"{path}{queryParameter}");
            return HttpContext.Current.Request.IsSecureConnection
                ? new HtmlString(cssTagBuilder.ToString())
                : new HtmlString(cssTagBuilder.ToString().Replace("http", "https"));
        }
    }
}