using System.Web.Mvc;
using WAFC.Bootstrap.ControlModels;
using WAFC.Bootstrap.Controls;
using WAFC.Bootstrap.TypeExtensions;

namespace WAFC.Bootstrap.Renderers
{
    internal static partial class Renderer
    {
        public static string RenderFormGroupCustom(HtmlHelper html, string input, BootstrapLabelModel labelModel)
        {
            string label = Renderer.RenderLabel(html, labelModel ?? new BootstrapLabelModel
            {
                htmlFieldName = labelModel.htmlFieldName,
                metadata = labelModel.metadata,
                htmlAttributes = new { @class = "control-label" }.ToDictionary()
            });

            bool fieldIsValid = true;
            if (labelModel != null && labelModel.htmlFieldName != null) fieldIsValid = html.ViewData.ModelState.IsValidField(labelModel.htmlFieldName);
            return new BootstrapFormGroup(input, label, FormGroupType.textboxLike, fieldIsValid).ToHtmlString();
        }
    }
}
