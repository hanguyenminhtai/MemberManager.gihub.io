using QuanLyDoanVien.Common;
using QuanLyDoanVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyDoanVien.Controllers
{
    public class NhanThucDangController : Admin_ConfirmController
    {
        // GET: NhanThucDang
        public ActionResult Index()
        {
            return View();
        }
        public LinqDataContext db;
        public NhanThucDangController() // Khởi tạo kết nối SQL
        {
            db = new LinqDataContext();
        }
        public JsonResult GetAll()
        {
            var dt = (from a in db.DeXuats join b in db.DoanViens on a.IDDoanVien equals b.ID
                      where a.Loai == 1 
                      select new DeXuatModel
                      {
                          ID = a.ID,
                          IDDoanVien = a.IDDoanVien,
                          TenDoanVien=b.HoTen,
                          NoiDung = a.NoiDung,
                          NgayThang = a.NgayThang.ToString("yyyy/MM/dd", new System.Globalization.CultureInfo("en-EN")),
                          Loai = a.Loai,
                          TrangThai = a.TrangThai.ToString(),
                          NguoiDeXuat = a.NguoiDeXuat
                      }).ToList();
            return Json(dt, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Add(DeXuatModel m)
        {
            ResponseBase res = new ResponseBase();
            try
            {
                var ses = (UserSave)Session[Constant.USER_SESSION];
                DeXuat dt = new DeXuat
                {
                    IDDoanVien = m.IDDoanVien,
                    NoiDung = m.NoiDung,
                    NgayThang = DateTime.Parse(m.NgayThang),
                    Loai = 1,
                    TrangThai=2,
                    NguoiDeXuat=ses.HoTen
                };
                db.DeXuats.InsertOnSubmit(dt);
                db.SubmitChanges();
                res.Status = StatusID.Success;
                res.Message = "Đã thêm";

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
        public JsonResult Update(DeXuatModel m)
        {
            ResponseBase res = new ResponseBase();
            try
            {
                var dt = (from a in db.DeXuats
                          where a.ID == m.ID
                          select a).FirstOrDefault();
                dt.IDDoanVien = m.IDDoanVien;
                dt.NoiDung = m.NoiDung;
                dt.NgayThang =DateTime.Parse(m.NgayThang);
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
       
        public JsonResult Update2(DeXuatModel m)
        {
            ResponseBase res = new ResponseBase();
            try
            {
                var dt = (from a in db.DeXuats
                          where a.ID == m.ID
                          select a).FirstOrDefault();
                dt.TrangThai = 2;
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
                var dt = (from a in db.DeXuats
                          where a.ID == ID
                          select a).FirstOrDefault();
                db.DeXuats.DeleteOnSubmit(dt);
                db.SubmitChanges();
                res.Status = StatusID.Success;
                res.Message = "Đã xoá thành công";

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