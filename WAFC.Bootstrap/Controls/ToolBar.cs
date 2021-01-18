using System.Collections.Generic;
using WAFC.Bootstrap.Infrastructure;

namespace WAFC.Bootstrap
{
    public class ToolBar : HtmlElement
    {
        public ToolBar()
            : base("div")
        {
            EnsureClass("btn-toolbar");
        }

        public ToolBar HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }

        public ToolBar HtmlAttributes(object htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }
    }
}
