﻿using System.Web.Mvc;
using WAFC.Bootstrap.ControlModels;
using WAFC.Bootstrap.Controls;
using WAFC.Bootstrap.TypeExtensions;

namespace WAFC.Bootstrap.Renderers
{
    internal static partial class Renderer
    {
        public static string RenderFormGroupPassword(HtmlHelper html, BootstrapTextBoxModel inputModel, BootstrapLabelModel labelModel)
        {
            var input = Renderer.RenderTextBox(html, inputModel, true);

            var widthlg = "";
            if (inputModel.LabelWidthLg != 0)
            {
                var width = inputModel.LabelWidthLg.ToString();
                widthlg = " col-lg-" + width;
            }

            var widthMd = "";
            if (inputModel.LabelWidthMd != 0)
            {
                var width = inputModel.LabelWidthMd.ToString();
                widthMd = " col-md-" + width;
            }

            var widthSm = "";
            if (inputModel.LabelWidthSm != 0)
            {
                var width = inputModel.LabelWidthSm.ToString();
                widthSm = " col-sm-" + width;
            }

            var widthXs = "";
            if (inputModel.LabelWidthXs != 0)
            {
                var width = inputModel.LabelWidthXs.ToString();
                widthXs = " col-xs-" + width;
            }

            string label = Renderer.RenderLabel(html, labelModel ?? new BootstrapLabelModel
            {
                htmlFieldName = inputModel.htmlFieldName,
                metadata = inputModel.metadata,
                htmlAttributes = new { @class = "control-label" + widthlg + widthMd + widthSm + widthXs }.ToDictionary()
            });

            bool fieldIsValid = true;
            if(inputModel != null) fieldIsValid = html.ViewData.ModelState.IsValidField(inputModel.htmlFieldName);
            return new BootstrapFormGroup(input, label, FormGroupType.textboxLike, fieldIsValid).ToHtmlString();
        }
    }
}
