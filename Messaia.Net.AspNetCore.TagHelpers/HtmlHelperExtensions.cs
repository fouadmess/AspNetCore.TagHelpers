///-----------------------------------------------------------------
///   Author:         fouad
///   AuthorUrl:      http://veritas-data.de
///   Date:           21.12.2018 11:06:57
///   Copyright (©)   2018, VERITAS DATA GmbH, all Rights Reserved. 
///                   No part of this document may be reproduced 
///                   without VERITAS DATA GmbH's express consent. 
///-----------------------------------------------------------------
namespace Messaia.Net.AspNetCore.TagHelpers
{
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text.Encodings.Web;

    /// <summary>
    /// HtmlHelperExtensions class
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Adds a resource (Javascript, CSS, ...)
        /// </summary>
        /// <param name="HtmlHelper"></param>
        /// <param name="resourceType"></param>
        /// <param name="Template"></param>
        /// <returns></returns>
        public static HtmlString AddResource(this IHtmlHelper HtmlHelper, PageResourceType resourceType, Func<object, HelperResult> Template)
        {
            if (!(HtmlHelper.ViewContext.HttpContext.Items[resourceType] is Dictionary<int, Func<object, HelperResult>> allItems))
            {
                allItems = new Dictionary<int, Func<object, HelperResult>>();
                HtmlHelper.ViewContext.HttpContext.Items[resourceType] = allItems;
            }

            int hash = -1;
            using (var writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                Template(null).WriteTo(writer, HtmlEncoder.Default);
                hash = writer.ToString().GetHashCode();
            }

            if (!allItems.ContainsKey(hash))
            {
                allItems.Add(hash, Template);
            }

            return new HtmlString(string.Empty);
        }

        /// <summary>
        /// Renders a resource (Javascript, CSS, ...)
        /// </summary>
        /// <param name="HtmlHelper"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public static HtmlString RenderResources(this IHtmlHelper HtmlHelper, PageResourceType resourceType)
        {
            if (HtmlHelper.ViewContext.HttpContext.Items[resourceType] != null)
            {
                var resources = (Dictionary<int, Func<object, HelperResult>>)HtmlHelper.ViewContext.HttpContext.Items[resourceType];
                foreach (var resource in resources?.Values)
                {
                    HtmlHelper.ViewContext.Writer.Write(resource(null));
                }
            }

            return new HtmlString(string.Empty);
        }
    }
}