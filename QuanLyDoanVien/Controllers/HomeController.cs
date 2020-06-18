using QuanLyDoanVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace QuanLyDoanVien.Controllers
{
    public class HomeController : Admin_ConfirmController
    {
        public LinqDataContext db;
        public HomeController() // Khởi tạo kết nối SQL
        {
            db = new LinqDataContext();
        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}