using System.Web.Mvc;
using WAFC.Bootstrap.BootstrapMethods;

namespace WAFC.Bootstrap
{
    public static class BootstrapHtmlExtension
    {
        public static Bootstrap<TModel> Bootstrap<TModel>(this HtmlHelper<TModel> helper)
        {
            return new Bootstrap<TModel>(helper);
        }
    }
}
