using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcWebpack.MvcExtensions
{
    public static class HtmlHelperExtensions
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
            string path = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~/Script/build/js/{bundleName}-bundle.js");
            string queryParameter = $"?noBC={DateTime.Now.Ticks}"; // noBC = no bundle caching
            jsTagBuilder.Attributes.Add("src", toAbsoluteUrl ? $"{new Uri(HttpContext.Current.Request.Url, path)}{queryParameter}" : $"{path}{queryParameter}");
            //return HttpContext.Current.Request.IsSecureConnection
                //? new HtmlString(jsTagBuilder.ToString())
                //: new HtmlString(jsTagBuilder.ToString().Replace("http", "https"));
            return new HtmlString(jsTagBuilder.ToString());
        }

        public static IHtmlString WebPackCss(this HtmlHelper helper, string appName, bool toAbsoluteUrl = true)
        {
            var cssTagBuilder = new TagBuilder("link");
            cssTagBuilder.Attributes.Add("rel", "stylesheet");
            cssTagBuilder.Attributes.Add("stylesheet", "text/css");
            if (ConfigurationManager.AppSettings["WebpackMode"].ToLower() == "local")
            {
                string port = ConfigurationManager.AppSettings["WebpackPort"] ?? "3000";
                cssTagBuilder.Attributes.Add("href", $"http://localhost:{port}/static/css/{appName}-bundle.css");
                return new HtmlString(cssTagBuilder.ToString());
            }
            string path = new UrlHelper(HttpContext.Current.Request.RequestContext).Content($"~/Script/build/css/{appName}-bundle.css");
            string queryParameter = $"?noBC={DateTime.Now.Ticks}"; // noBC = no bundle caching
            cssTagBuilder.Attributes.Add("href", toAbsoluteUrl ? $"{new Uri(HttpContext.Current.Request.Url, path)}{queryParameter}" : $"{path}{queryParameter}");
            //return HttpContext.Current.Request.IsSecureConnection
            //? new HtmlString(cssTagBuilder.ToString())
            //: new HtmlString(cssTagBuilder.ToString().Replace("http", "https"));
            return new HtmlString(cssTagBuilder.ToString());
        }
    }
}