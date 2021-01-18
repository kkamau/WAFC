using System.Web.Mvc;
using WAFC.Bootstrap.Infrastructure;

namespace WAFC.Bootstrap.Controls
{
    public class TableRowBuilder<TModel> : BuilderBase<TModel, TableRow>
    {
        internal TableRowBuilder(HtmlHelper<TModel> htmlHelper, TableRow tableRow)
            : base(htmlHelper, tableRow)
        {
        }
    }
}
