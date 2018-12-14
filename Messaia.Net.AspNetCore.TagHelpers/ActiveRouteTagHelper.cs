///-----------------------------------------------------------------
///   Author:         fouad
///   AuthorUrl:      http://veritas-data.de
///   Date:           14.12.2018 04:00:50
///   Copyright (©)   2018, VERITAS DATA GmbH, all Rights Reserved. 
///                   No part of this document may be reproduced 
///                   without VERITAS DATA GmbH's express consent. 
///-----------------------------------------------------------------
namespace Messaia.Net.AspNetCore.TagHelpers
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// ActiveRouteTagHelper class
    /// </summary>
    [HtmlTargetElement(Attributes = "is-active-route")]
    public class ActiveRouteTagHelper : TagHelper
    {
        /// <summary>
        /// The name of the action method.
        /// </summary>
        [HtmlAttributeName("asp-page")]
        public string Page { get; set; }

        /// <summary>
        /// Additional parameters for the route.
        /// </summary>
        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public IDictionary<string, string> RouteValues { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Mvc.Rendering.ViewContext" /> for the current request.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            if (this.ShouldBeActive())
            {
                this.MakeActive(output);
            }

            output.Attributes.RemoveAll("is-active-route");
        }

        /// <summary>
        /// Checks if the current link is the current page
        /// </summary>
        /// <returns></returns>
        private bool ShouldBeActive()
        {
            /* Get current action */
            var currentAction = ViewContext.RouteData.Values["page"].ToString();
            if (!string.IsNullOrWhiteSpace(Page) && Page.ToLower() != currentAction.ToLower())
            {
                return false;
            }

            /* Compare current action with routes values */
            foreach (KeyValuePair<string, string> routeValue in RouteValues)
            {
                if (!ViewContext.RouteData.Values.ContainsKey(routeValue.Key) ||
                    ViewContext.RouteData.Values[routeValue.Key].ToString() != routeValue.Value)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Adds the css class 'active' to the current link
        /// </summary>
        /// <param name="output"></param>
        private void MakeActive(TagHelperOutput output)
        {
            var classAttr = output.Attributes.FirstOrDefault(a => a.Name == "class");
            if (classAttr == null)
            {
                classAttr = new TagHelperAttribute("class", "active");
                output.Attributes.Add(classAttr);
            }
            else if (classAttr.Value == null || classAttr.Value.ToString().IndexOf("active") < 0)
            {
                output.Attributes.SetAttribute("class", classAttr.Value == null ? "active" : classAttr.Value.ToString() + " active");
            }
        }
    }
}