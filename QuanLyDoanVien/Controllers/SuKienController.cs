using QuanLyDoanVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace QuanLyDoanVien.Controllers
{
    public class SuKienController : Admin_ConfirmController
    {
        public LinqDataContext db;
        public SuKienController() // Khởi tạo kết nối SQL
        {
            db = new LinqDataContext();
        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetAll()
        {
            var dt = (from a in db.SuKiens
                      select new SuKienModel
                      {
                          GhiChu = a.GhiChu.Trim(),
                          ID = a.ID,
                          BatDau = a.BatDau.ToString("yyyy/MM/dd hh:mm:ss tt", new System.Globalization.CultureInfo("en-EN")),
                          DiaDiem = a.DiaDiem,
                          NoiDung = a.NoiDung,
                          Ten = a.Ten
                      }).ToList();
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TraCuu(int ID)
        {
            var lst = (from a in db.DangKies
                       where a.IDSuKien == ID
                       select new DoanVienModel
                       {
                           HoTen = a.DoanVien.HoTen,
                           ID = a.DoanVien.ID,
                           NgaySinh = a.DoanVien.NgaySinh.ToShortDateString(),
                           DiaChi = a.DoanVien.DiaChi,
                           NgayVaoDoan = a.DoanVien.NgayVaoDoan.ToShortDateString(),
                           Email = a.DoanVien.Email,
                           GioiTinh = a.DoanVien.GioiTinh ? "Nam" : "Nữ",
                           SoDienThoai = a.DoanVien.SoDienThoai
                       }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Add(SuKienModel m)
        {
            ResponseBase res = new ResponseBase();
            try
            {
                SuKien dt = new SuKien
                {
                    Ten = m.Ten,
                    NoiDung = m.NoiDung,
                    DiaDiem = m.DiaDiem,
                    BatDau = DateTime.Parse(m.BatDau),
                    GhiChu = m.GhiChu
                };
                db.SuKiens.InsertOnSubmit(dt);
                db.SubmitChanges();
                res.Status = StatusID.Success;
                res.Message = "Đã thêm sự kiện";

            }
            catch (Exception e)
            {
                res.Status = StatusID.InternalServer;
                res.Message = e.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Update(SuKienModel m)
        {
            ResponseBase res = new ResponseBase();
            try
            {
                var dt = (from a in db.SuKiens
                          where a.ID == m.ID
                          select a).FirstOrDefault();
                dt.Ten = m.Ten;
                dt.NoiDung = m.NoiDung;
                dt.DiaDiem = m.DiaDiem;
                dt.BatDau = DateTime.Parse(m.BatDau);
                dt.GhiChu = m.GhiChu;
                db.SubmitChanges();
                res.Status = StatusID.Success;
                res.Message = "Thông tin đã cập nhật thành công";
            }
            catch (Exception e)
            {
                res.Status = StatusID.InternalServer;
                res.Message = e.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int ID)
        {
            ResponseBase res = new ResponseBase();
            try
            {
                var check = (from a in db.DangKies
                             where a.IDSuKien == ID
                             select a).ToList();
                if (check.Count > 0)
                {
                    res.Status = StatusID.InternalServer;
                    res.Message = "Tác vụ thất bại! Không thể xóa sự kiện đã đăng ký";
                }
                else
                {
                    var dt = (from a in db.SuKiens
                              where a.ID == ID
                              select a).FirstOrDefault();
                    db.SuKiens.DeleteOnSubmit(dt);
                    db.SubmitChanges();
                    res.Status = StatusID.Success;
                    res.Message = "Sự kiện đã được xóa thành công";
                }
            }
            catch (Exception e)
            {
                res.Status = StatusID.InternalServer;
                res.Message = e.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SendMail(int ID)
        {
            ResponseBase res = new ResponseBase();
            try
            {
                var sk = (from a in db.SuKiens
                          where a.ID == ID
                          select a).FirstOrDefault();
                var dt = (from a in db.DoanViens
                          select a).ToList();
                if (sk != null && dt.Count() > 0)
                {
                    string email = WebConfigurationManager.AppSettings["Email_Support"]; // Lấy email từ web.config
                    string password = WebConfigurationManager.AppSettings["PassWord_Email_Support"]; //Lấy mật khẩu từ web.config
                    var subject = "Thông báo sự kiện " + sk.Ten;
                    var body = "Địa điểm: " + sk.DiaDiem;
                    body += "Bắt đầu: " + sk.BatDau;
                    body += "<br/> Nội sung sự kiện: <hr/> " + sk.NoiDung;
                    var a = Utils.SendEmailBCC(subject, body, password, email, dt);
                    if (a.Status == StatusID.Success)
                    {
                        res.Status = StatusID.Success;
                        res.Message = "Thông tin sự kiện đã được gửi đến email của tất cả đoàn viên";
                    }
                    else
                    {
                        res.Status = StatusID.InternalServer;
                        res.Message = a.Message;
                    }

                }
                else
                {
                    res.Status = StatusID.InternalServer;
                    res.Message = "Không tìm thấy sinh viên hoặc sự kiện";
                }


            }
            catch (Exception e)
            {
                res.Status = StatusID.InternalServer;
                res.Message = e.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}