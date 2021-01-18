using System.Web.Mvc;
using WAFC.Bootstrap.ControlModels;
using WAFC.Bootstrap.Controls;
using WAFC.Bootstrap.Infrastructure.Enums;
using WAFC.Bootstrap.TypeExtensions;

namespace WAFC.Bootstrap.Renderers
{
    internal static partial class Renderer
    {
        public static string RenderFormGroupSelectElement(HtmlHelper html, BootstrapSelectElementModel inputModel, BootstrapLabelModel labelModel, BootstrapInputType inputType)
        {
            string input = string.Empty;
            
            if(inputType == BootstrapInputType.DropDownList)
                input = Renderer.RenderSelectElement(html, inputModel, BootstrapInputType.DropDownList);

            if (inputType == BootstrapInputType.ListBox)
                input = Renderer.RenderSelectElement(html, inputModel, BootstrapInputType.ListBox);

            string label = Renderer.RenderLabel(html, labelModel ?? new BootstrapLabelModel
            {
                htmlFieldName = inputModel.htmlFieldName,
                metadata = inputModel.metadata,
                htmlAttributes = new { @class = "control-label" }.ToDictionary()
            });

            bool fieldIsValid = true;
            if(inputModel != null) fieldIsValid = html.ViewData.ModelState.IsValidField(inputModel.htmlFieldName);
            return new BootstrapFormGroup(input, label, FormGroupType.textboxLike, fieldIsValid).ToHtmlString();
        }
    }
}
