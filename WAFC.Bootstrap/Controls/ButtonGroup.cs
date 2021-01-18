using System.Collections.Generic;
using WAFC.Bootstrap.Infrastructure;

namespace WAFC.Bootstrap
{
    public class ButtonGroup : HtmlElement
    {
        public ButtonGroup()
            : base("div")
        {
            EnsureClass("btn-group");
        }

        public ButtonGroup HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }

        public ButtonGroup HtmlAttributes(object htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }
    }
}
