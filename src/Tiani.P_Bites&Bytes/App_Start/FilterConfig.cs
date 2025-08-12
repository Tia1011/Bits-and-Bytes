using System.Web;
using System.Web.Mvc;

namespace Tiani.P_Bites_Bytes
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
