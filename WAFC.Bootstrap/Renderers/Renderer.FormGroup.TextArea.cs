using System.Web.Mvc;
using WAFC.Bootstrap.ControlModels;
using WAFC.Bootstrap.Controls;
using WAFC.Bootstrap.TypeExtensions;

namespace WAFC.Bootstrap.Renderers
{
    internal static partial class Renderer
    {
        public static string RenderFormGroupTextArea(HtmlHelper html, BootstrapTextAreaModel inputModel, BootstrapLabelModel labelModel)
        {
            var input = Renderer.RenderTextArea(html, inputModel);

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
