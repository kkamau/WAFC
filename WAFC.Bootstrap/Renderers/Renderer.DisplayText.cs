using System.Web.Mvc;
using System.Web.Mvc.Html;
using WAFC.Bootstrap.ControlModels;
using WAFC.Bootstrap.TypeExtensions;

namespace WAFC.Bootstrap.Renderers
{
    internal static partial class Renderer
    {
        public static string RenderDisplayText(HtmlHelper html, BootstrapDisplayTextModel model)
        {
            var input = html.DisplayText(model.htmlFieldName);

            TagBuilder containerDiv = new TagBuilder("div");
            containerDiv.MergeAttributes(model.htmlAttributes.FormatHtmlAttributes());
            containerDiv.AddCssStyle("padding-top", "5px");
            containerDiv.InnerHtml = input.ToHtmlString();

            return containerDiv.ToString(TagRenderMode.Normal);
        }
    }
}
