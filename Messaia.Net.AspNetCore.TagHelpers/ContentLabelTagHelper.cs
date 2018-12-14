///-----------------------------------------------------------------
///   Author:         Fouad Messaia
///   AuthorUrl:      http://veritas-data.de
///   Date:           02.01.2018
///   Copyright (©)   2016, VERITAS DATA GmbH, all Rights Reserved. 
///                   No part of this document may be reproduced 
///                   without VERITAS DATA GmbH's express consent. 
///-----------------------------------------------------------------
namespace Messaia.Net.AspNetCore.TagHelpers
{
    using Microsoft.AspNetCore.Mvc.TagHelpers;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// ContentLabelTagHelper class
    /// </summary>
    [HtmlTargetElement("label", Attributes = "asp-for,asp-content")]
    public class ContentLabelTagHelper : LabelTagHelper
    {
        #region Fields

        /// <summary>
        /// For attribute name
        /// </summary>
        private const string ForAttributeName = "asp-for";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="ContentLabelTagHelper"/> class.
        /// </summary>
        /// <param name="generator"></param>
        public ContentLabelTagHelper(IHtmlGenerator generator) : base(generator) { }

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <remarks>Does nothing if <see cref="For"/> is <c>null</c>.</remarks>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var tagBuilder = Generator.GenerateLabel(ViewContext, For.ModelExplorer, For.Name, labelText: null, htmlAttributes: null);
            if (tagBuilder != null)
            {
                output.MergeAttributes(tagBuilder);

                /* We check for whitespace to detect scenarios */
                var childContent = await output.GetChildContentAsync();
                if (childContent.IsEmptyOrWhiteSpace)
                {
                    /* Provide default label text (if any) since there was nothing useful in the Razor source. */
                    if (tagBuilder.HasInnerHtml)
                    {
                        output.Content.SetHtmlContent(tagBuilder.InnerHtml);
                    }
                }
                else
                {
                    output.Content.SetHtmlContent(childContent).AppendHtml(tagBuilder.InnerHtml);
                }
            }
        }

        #endregion
    }
}