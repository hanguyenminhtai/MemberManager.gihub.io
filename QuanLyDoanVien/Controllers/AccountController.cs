using QuanLyDoanVien.Models;
using QuanLyDoanVien.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyDoanVien.Controllers
{
    public class AccountController : Controller
    {
        public LinqDataContext db;
        public AccountController() // Khởi tạo kết nối SQL
        {
            db = new LinqDataContext();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DoanVien()
        {
            return View();
        }
        public JsonResult LoginUser(Account model)
        {
            var data = new ResponseBase();
            try
            {

                var rs = (from a in db.Users
                          where a.UserName == model.UserName && a.Password == model.UserPassword
                          select a).ToList();

                if (rs.Count() != 1)
                {
                    data.Status = StatusID.AccessDenied;
                    data.Message = "Thông tin đăng nhập không chính xác";
                }
                else
                {
                    // đăng nhập thành công
                    var dt = rs.FirstOrDefault();
                    var us = new UserSave();
                    us.HoTen = dt.HoTen;
                    us.ID = dt.ID;
                    us.UserName = dt.UserName;
                    us.Password = dt.Password;
                    Session.Add(Constant.USER_SESSION, us);
                    data.Status = StatusID.Success;
                    data.Message = "Đăng nhập thành công!";
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                data.Status = StatusID.ServerBusy;
                data.Message = "Lỗi kết nối " + e;
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult LoginDoanVien(Account model)
        {
            var data = new ResponseBase();
            try
            {

                var rs = (from a in db.DoanViens
                          where a.Email.Trim() == model.EmailAdress.Trim() && a.Password.Trim() == model.UserPassword.Trim()
                          select a).ToList();

                if (rs.Count() != 1)
                {
                    data.Status = StatusID.AccessDenied;
                    data.Message = "Thông tin đăng nhập không chính xác";
                }
                else
                {
                    // đăng nhập thành công
                    var dt = rs.FirstOrDefault();
                    var us = new DoanVienSave();
                    us.HoTen = dt.HoTen;
                    us.ID = dt.ID;
                    us.Email = dt.Email;
                    us.DiaChi = dt.DiaChi.Trim();
                    us.GioiTinh = dt.GioiTinh ? "Nam" : "Nữ";
                    us.NgaySinh = dt.NgaySinh.ToShortDateString();
                    us.NgayVaoDoan = dt.NgayVaoDoan.ToShortDateString();
                    us.NhanThucChinhTri = dt.NhanThucChinhTri;
                    us.RenLuyenCaNhan = dt.RenLuyenCaNhan;
                    us.XepLoai = dt.XepLoai;
                    us.ChuyenMonNghiepVu = dt.ChuyenMonNghiepVu;
                    us.MSSV = dt.MSSV;
                    us.NoiVaoDoan = dt.NoiVaoDoan;
                    us.LaBiThu = dt.LaBiThu;
                    us.SoDienThoai = dt.SoDienThoai;
                    Session.Add(Constant.CUS_SESSION, us);
                    data.Status = StatusID.Success;
                    data.Message = "Đăng nhập thành công!";
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                data.Status = StatusID.ServerBusy;
                data.Message = "Lỗi kết nối " + e;
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}