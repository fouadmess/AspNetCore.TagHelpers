///-----------------------------------------------------------------
///   Author:         Messaia
///   AuthorUrl:      http://veritas-data.de
///   Date:           29.01.2017 16:42:36
///   Copyright (©)   2017, VERITAS DATA GmbH, all Rights Reserved. 
///                   No part of this document may be reproduced 
///                   without VERITAS DATA GmbH's express consent. 
///-----------------------------------------------------------------
namespace Messaia.Net.AspNetCore.TagHelpers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using System;
    using System.Text;

    /// <summary>
    /// MenuLinkTagHelper class.
    /// </summary>
    [HtmlTargetElement("menulink", Attributes = "asp-controller, asp-action, menu-text")]
    public class MenuLinkTagHelper : TagHelper
    {
        #region Properties

        /// <summary>
        /// Gets or sets the AspController
        /// </summary>
        public string AspController { get; set; }

        /// <summary>
        /// Gets or sets the AspAction
        /// </summary>
        public string AspAction { get; set; }

        /// <summary>
        /// Gets or sets the MenuText
        /// </summary>
        public string MenuText { get; set; }

        /// <summary>
        /// Gets or sets the ViewContext
        /// </summary>
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Gets or sets the UrlHelperFactory
        /// </summary>
        public IUrlHelperFactory UrlHelperFactory { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="MenuLinkTagHelper"/> class.
        /// </summary>
        public MenuLinkTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            this.UrlHelperFactory = urlHelperFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Synchronously executes the Microsoft.AspNetCore.Razor.TagHelpers.TagHelper with
        /// the given context and output.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            StringBuilder stringBuilder = new StringBuilder();

            /* 
            Generates a URL with an absolute path for an action method, which contains 
            the specified action and controller names. 
            */
            var menuUrl = UrlHelperFactory.GetUrlHelper(ViewContext).Action(this.AspAction, this.AspController);

            /* Menu classes */
            var classes = new StringBuilder("nav-item");

            /* The HTML element's tag name */
            output.TagName = "li";

            /* Create the menu link */
            var a = new TagBuilder("a");
            a.MergeAttribute("href", menuUrl);
            a.MergeAttribute("title", this.MenuText);
            a.MergeAttribute("class", "nav-link");
            a.InnerHtml.Append(this.MenuText);

            /* Get the current controller name and action */
            var routeData = ViewContext.RouteData.Values;
            var currentController = routeData["controller"];
            var currentAction = routeData["action"];

            /* Compare the controller names and the actions */
            if (string.Equals(this.AspAction, currentAction as string, StringComparison.OrdinalIgnoreCase)
                && string.Equals(this.AspController, currentController as string, StringComparison.OrdinalIgnoreCase))
            {
                classes.Append(" active");
            }

            output.Attributes.Add("class", classes.ToString().Trim());
            output.Content.AppendHtml(a);
        }

        #endregion
    }
}