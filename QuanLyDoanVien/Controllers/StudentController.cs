using QuanLyDoanVien.Common;
using QuanLyDoanVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyDoanVien.Controllers
{
    public class StudentController : Student_ConfirmController
    {
        public LinqDataContext db;
        public StudentController() // Khởi tạo kết nối SQL
        {
            db = new LinqDataContext();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TraCuu()
        {
            return View();
        }
        public ActionResult MuaAo()
        {
            return View();
        }
        public ActionResult DuyetMuaAo()
        {
            var ses = (DoanVienSave)Session[Constant.CUS_SESSION];
            if (ses.LaBiThu == true)
            {
                return View();
            }
            else
            {
                return View("Error");
            }
        }
        public ActionResult DeXuat()
        {
            var ses = (DoanVienSave)Session[Constant.CUS_SESSION];
            if (ses.LaBiThu == true)
            {
                return View();
            }
            else
            {
                return View("Error");
            }
        }
        public JsonResult GetUser()
        {
            DoanVienSave datas = new DoanVienSave();
            var ses = (DoanVienSave)Session[Constant.CUS_SESSION];
            if (ses == null)
            {
                datas.ID = 0;
            }
            else
            {
                datas.ID = ses.ID;
                datas.HoTen = ses.HoTen;
                datas.Email = ses.Email;
            }
            return Json(datas, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllSuKien()
        {
            var ses = (DoanVienSave)Session[Constant.CUS_SESSION];
            var dt = (from a in db.DoanVien_Get_SuKien(ses.ID)
                      select new SuKienDoanVienModel
                      {
                          GhiChu = a.GhiChu,
                          ID = a.ID,
                          BatDau = a.BatDau.ToString(),
                          DiaDiem = a.DiaDiem,
                          NoiDung = a.NoiDung,
                          Ten = a.Ten,
                          TrangThai = a.TrangThai.GetValueOrDefault(),
                          TenTrangThai = a.TrangThai.GetValueOrDefault() == 0 ? "Chưa đăng ký" : "Đã đăng ký"
                      }).ToList();
            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TraCuuDanhSach(int ID)
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
        public JsonResult DangKySuKien(int IDSuKien)
        {
            var ses = (DoanVienSave)Session[Constant.CUS_SESSION];
            var lst = (from a in db.DangKies
                       where a.IDSuKien == IDSuKien && a.IDDoanVien == ses.ID
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
            if (lst.Count > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                DangKy dk = new DangKy
                {
                    IDDoanVien = ses.ID,
                    IDSuKien = IDSuKien,
                    NgayDangKy = DateTime.Now
                };
                db.DangKies.InsertOnSubmit(dk);
                db.SubmitChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult HuyDangKySuKien(int IDSuKien)
        {
            var ses = (DoanVienSave)Session[Constant.CUS_SESSION];
            var dt = (from a in db.DangKies
                      where a.IDSuKien == IDSuKien && a.IDDoanVien == ses.ID
                      select a).FirstOrDefault();
            db.DangKies.DeleteOnSubmit(dt);
            db.SubmitChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInfor()
        {
            var ses = (DoanVienSave)Session[Constant.CUS_SESSION];
            return Json(ses, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangePassWordUser(string PasswordOld, string PasswordNew)
        {
            ResponseBase res = new ResponseBase();
            var ses = (DoanVienSave)Session[Constant.CUS_SESSION];
            try
            {
                var dt = (from a in db.DoanViens
                          where a.ID == ses.ID && a.Password == PasswordOld
                          select a).ToList();
                if (dt.Count == 1)
                {
                    dt.FirstOrDefault().Password = PasswordNew;
                    db.SubmitChanges();
                    res.Status = StatusID.Success;
                    res.Message = "Mật khẩu đã được thay đổi";
                }
                else
                {
                    res.Status = StatusID.AccessDenied;
                    res.Message = "Mật khẩu cũ không đúng!";
                }
            }
            catch (Exception ex)
            {
                res.Status = StatusID.AccessDenied;
                res.Message = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MuaAoAdd(MuaAoModel m)
        {
            ResponseBase res = new ResponseBase();
            try
            {
                MuaAo dt = new MuaAo
                {
                    IDDoanVien = m.IDDoanVien,
                    NoiDung = m.NoiDung,
                    SoLuong = m.SoLuong,
                    NgayThang = DateTime.Now,
                    TrangThai = 1,
                };
                db.MuaAos.InsertOnSubmit(dt);
                db.SubmitChanges();
                res.Status = StatusID.Success;
                res.Message = "Đã đặt thành công";

            }
            catch (Exception e)
            {
                res.Status = StatusID.InternalServer;
                res.Message = e.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllMuaAo()
        {
            var dt = (from a in db.MuaAos
                      join b in db.DoanViens on a.IDDoanVien equals b.ID
                      select new MuaAoModel
                      {
                          ID = a.ID,
                          IDDoanVien = a.IDDoanVien,
                          HoTen = b.HoTen,
                          SoLuong = a.SoLuong,
                          NoiDung = a.NoiDung,
                          NgayThang = a.NgayThang.ToString("yyyy/MM/dd", new System.Globalization.CultureInfo("en-EN")),
                          TrangThai = a.TrangThai
                      }).ToList();
            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DuyetMuaAoAdd(MuaAoModel m)
        {
            ResponseBase res = new ResponseBase();
            try
            {
                var dt = (from a in db.MuaAos
                          where a.ID == m.ID
                          select a).FirstOrDefault();
                dt.TrangThai = m.TrangThai;
                db.SubmitChanges();
                res.Status = StatusID.Success;
                res.Message = "Thành công";
            }
            catch (Exception e)
            {
                res.Status = StatusID.InternalServer;
                res.Message = e.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(MuaAoModel m)
        {
            ResponseBase res = new ResponseBase();
            try
            {
                var dt = (from a in db.MuaAos
                          where a.ID == m.ID
                          select a).FirstOrDefault();
                dt.IDDoanVien = m.IDDoanVien;
                dt.TrangThai = 3;
                db.SubmitChanges();
                res.Status = StatusID.Success;
                res.Message = "Đã huỷ đơn hàng";

            }
            catch (Exception e)
            {
                res.Status = StatusID.InternalServer;
                res.Message = e.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DoanVien_GetAll()
        {
            var lst = (from a in db.DoanViens
                       select new DoanVienModel
                       {
                           HoTen = a.HoTen,
                           ID = a.ID,
                           NgaySinh = a.NgaySinh.ToShortDateString(),
                           DiaChi = a.DiaChi,
                           NgayVaoDoan = a.NgayVaoDoan.ToShortDateString(),
                           Email = a.Email,
                           GioiTinh = a.GioiTinh ? "Nam" : "Nữ",
                           SoDienThoai = a.SoDienThoai,
                           MSSV = a.MSSV
                       }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeXuatDV(DeXuatModel m)
        {
            ResponseBase res = new ResponseBase();
            try
            {
                var ses = (DoanVienSave)Session[Constant.CUS_SESSION];
                DeXuat dt = new DeXuat
                {
                    IDDoanVien = m.IDDoanVien,
                    NoiDung = m.NoiDung,
                    NgayThang = DateTime.Now,
                    Loai = m.Loai,
                    TrangThai = 1,
                    NguoiDeXuat = ses.HoTen
                };
                db.DeXuats.InsertOnSubmit(dt);
                db.SubmitChanges();
                res.Status = StatusID.Success;

                res.Message = "Đã gửi đề xuất cho quản trị viên";

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