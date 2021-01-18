using System.Web.Mvc;
using WAFC.Bootstrap.Infrastructure;

namespace WAFC.Bootstrap.Controls
{
    public class FormActionsBuilder<TModel> : BuilderBase<TModel, FormActions>
    {
        internal FormActionsBuilder(HtmlHelper<TModel> htmlHelper, FormActions formActions)
            : base(htmlHelper, formActions)
        {
        }
    }
}
