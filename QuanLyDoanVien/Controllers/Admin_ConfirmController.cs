using QuanLyDoanVien.Common;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuanLyDoanVien.Controllers
{
    public class Admin_ConfirmController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ses = (UserSave)Session[Constant.USER_SESSION];

            if (ses == null)
            {
                ModelState.AddModelError("", "Bạn chưa đăng nhập!");
                filterContext.Result =
                    new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            {"controller", "Account"},
                            {"action", "Index"}
                        });
            }
            base.OnActionExecuting(filterContext);
        }
    }
}