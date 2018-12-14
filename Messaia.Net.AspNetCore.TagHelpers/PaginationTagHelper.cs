///-----------------------------------------------------------------
///   Author:         Messaia
///   AuthorUrl:      http://veritas-data.de
///   Date:           02.02.2017 00:24:32
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
    using Messaia.Net.Pagination;

    /// <summary>
    /// PaginationTagHelper class.
    /// </summary>
    [HtmlTargetElement("pagination", Attributes = "asp-model")]
    public class PaginationTagHelper : TagHelper
    {
        #region Fields

        /// <summary>
        /// Represents the current controller
        /// </summary>
        private string currentController;

        /// <summary>
        /// Represents the current action
        /// </summary>
        private string currentAction;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the AspModel
        /// </summary>
        public IPagination AspModel { get; set; }

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
        public PaginationTagHelper(IUrlHelperFactory urlHelperFactory)
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
            if (this.AspModel != null)
            {
                this.currentController = ViewContext.RouteData.Values["controller"] as string;
                this.currentAction = ViewContext.RouteData.Values["action"] as string;

                /* The HTML element's tag name */
                output.TagName = "ul";
                output.Attributes.Add("class", "pagination");

                /* Add 'first' pagination element */
                var routeValues = this.AspModel.Page > 1 ? new { Page = this.AspModel.Page - 1, Limit = this.AspModel.PageSize } : null;
                output.Content.AppendHtml(this.BuildPaginationItem("Zurück", routeValues));

                /* Add pagination elements */
                for (int i = 1; i <= this.AspModel.PageCount; i++)
                {
                    output.Content.AppendHtml(this.BuildPaginationItem(i.ToString(), new { Page = i, Limit = this.AspModel.PageSize }, i == this.AspModel.Page));
                }

                /* Add 'last' pagination element */
                routeValues = this.AspModel.Page < this.AspModel.PageCount ? new { Page = this.AspModel.Page + 1, Limit = this.AspModel.PageSize } : null;
                output.Content.AppendHtml(this.BuildPaginationItem("Vorwärts", routeValues));
            }
        }

        /// <summary>
        /// Builds a pagination item
        /// </summary>
        /// <param name="text">The text of the item</param>
        /// <param name="values">The route values</param>
        /// <param name="active">Indecites whether this item is active</param>
        /// <returns></returns>
        private TagBuilder BuildPaginationItem(string text, object values, bool active = false)
        {
            /*
             * Generates a URL with an absolute path for an action method, 
             * which contains the specified action and controller names 
             */
            var menuUrl = UrlHelperFactory
                .GetUrlHelper(ViewContext)
                .Action(currentAction, currentController, values);

            /* Create the item link element */
            var a = new TagBuilder("a");
            a.MergeAttribute("href", menuUrl);
            a.AddCssClass("page-link");
            a.InnerHtml.Append(text);

            /* Create the list item element */
            var li = new TagBuilder("li");
            li.MergeAttribute("class", "page-item");

            /* Set active page item */
            if (active)
            {
                li.AddCssClass("active");
            }

            /* Set disabled page item */
            if (values == null)
            {
                li.AddCssClass("disabled");
            }

            /* Add the link element to the list item element */
            li.InnerHtml.AppendHtml(a);

            return li;
        }

        #endregion
    }
}