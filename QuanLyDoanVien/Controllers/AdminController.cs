using Newtonsoft.Json;
using QuanLyDoanVien.Common;
using QuanLyDoanVien.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace QuanLyDoanVien.Controllers
{
    public class AdminController : Admin_ConfirmController
    {
        public LinqDataContext db;
        public AdminController() // Khởi tạo kết nối SQL
        {
            db = new LinqDataContext();
        }
        public JsonResult ChangePassWordUser(string PasswordOld, string PasswordNew)
        {
            ResponseBase res = new ResponseBase();
            var ses = (UserSave)Session[Constant.USER_SESSION];
            try
            {
                var dt = (from a in db.Users
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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SinhVien()
        {
            return View();
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
                           MSSV = a.MSSV,
                           NoiVaoDoan = a.NoiVaoDoan,
                           TenXepLoai = getXepLoai(a.XepLoai),
                           XepLoai = a.XepLoai,
                           NhanThucChinhTri = a.NhanThucChinhTri,
                           RenLuyenCaNhan = a.RenLuyenCaNhan,
                           ChuyenMonNghiepVu = a.ChuyenMonNghiepVu,
                           LaBiThu = a.LaBiThu
                       }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DoanVien_Add(DoanVienModel m)
        {
            ResponseBase res = new ResponseBase();
            try
            {
                if (Utils.checkEmailDoanvienAdd(m.Email) && Utils.IsValidEmail(m.Email))
                {
                    Random generator = new Random();
                    String randomPass = generator.Next(0, 999999).ToString("D6");
                    DoanVien dt = new DoanVien
                    {
                        HoTen = m.HoTen,
                        DiaChi = m.DiaChi,
                        Email = m.Email,
                        GioiTinh = m.GioiTinh == "Nam",
                        NgaySinh = DateTime.Parse(m.NgaySinh),
                        NgayVaoDoan = DateTime.Parse(m.NgayVaoDoan),
                        Password = randomPass,
                        SoDienThoai = m.SoDienThoai,
                        NoiVaoDoan = m.NoiVaoDoan,
                        RenLuyenCaNhan = m.RenLuyenCaNhan,
                        NhanThucChinhTri = m.NhanThucChinhTri,
                        ChuyenMonNghiepVu = m.ChuyenMonNghiepVu,
                        MSSV = m.MSSV,
                        LaBiThu = m.LaBiThu,
                        XepLoai = getXepLoai(m)
                    };
                    db.DoanViens.InsertOnSubmit(dt);
                    db.SubmitChanges();
                    string email = WebConfigurationManager.AppSettings["Email_Support"]; // Lấy email từ web.config
                    string password = WebConfigurationManager.AppSettings["PassWord_Email_Support"]; //Lấy mật khẩu từ web.config
                    var subject = "Thông báo tài khoản";
                    var body = "Hệ thống xin chào đoàn viên" + m.HoTen + ", mật khẩu của bạn là: " + randomPass;
                    var a = Utils.SendEmail(m.Email, subject, body, password, email);
                    if (a.Status == StatusID.Success)
                    {
                        res.Status = StatusID.Success;
                        res.Message = "Đã thêm đoàn viên và gửi email thành công!";
                    }
                    else
                    {
                        res.Status = StatusID.ServerBusy;
                        res.Message = "Lỗi phát sinh trong quá trình tài khoản. Vui lòng thử lại sau";
                    }
                }
                else
                {
                    res.Status = StatusID.ServerBusy;
                    res.Message = "Email không đúng định dạng.";
                }


            }
            catch (Exception e)
            {
                res.Status = StatusID.InternalServer;
                res.Message = e.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string getXepLoai(int ID)
        {
            var TenXepLoai = string.Empty;
            switch (ID)
            {
                case 0:
                    TenXepLoai = "Chưa xếp loại";
                    break;
                case 1:
                    TenXepLoai = "Yếu kém";
                    break;
                case 2:
                    TenXepLoai = "Trung bình";
                    break;
                case 3:
                    TenXepLoai = "Khá";
                    break; ;
                default:
                    TenXepLoai = "Xuất sắc";
                    break;
            }
            return TenXepLoai;
        }
        public int getXepLoai(DoanVienModel m)
        {
            var tb = double.Parse(((m.NhanThucChinhTri + m.RenLuyenCaNhan + m.ChuyenMonNghiepVu) / 3).GetValueOrDefault().ToString());
            if (tb >= 8)
            {
                return 4;
            };
            if (tb >= 6.5)
            {
                return 3;
            };
            if (tb >= 5)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
        public JsonResult DoanVien_Edit(DoanVienModel m)
        {
            ResponseBase res = new ResponseBase();
            try
            {
                if (Utils.checkEmailDoanvien(m.ID, m.Email) && Utils.IsValidEmail(m.Email))
                {
                    var dt = (from x in db.DoanViens
                              where x.ID == m.ID
                              select x).FirstOrDefault();

                    dt.HoTen = m.HoTen;
                    dt.DiaChi = m.DiaChi;
                    dt.Email = m.Email;
                    dt.GioiTinh = m.GioiTinh == "Nam";
                    dt.NgaySinh = DateTime.Parse(m.NgaySinh);
                    dt.NgayVaoDoan = DateTime.Parse(m.NgayVaoDoan);
                    dt.SoDienThoai = m.SoDienThoai;
                    dt.XepLoai = getXepLoai(m);
                    dt.MSSV = m.MSSV;
                    dt.NoiVaoDoan = m.NoiVaoDoan;
                    dt.ChuyenMonNghiepVu = m.ChuyenMonNghiepVu;
                    dt.RenLuyenCaNhan = m.RenLuyenCaNhan;
                    dt.NhanThucChinhTri = m.NhanThucChinhTri;
                    dt.LaBiThu = m.LaBiThu;
                    db.SubmitChanges();
                    res.Status = StatusID.Success;
                    res.Message = "Thông tin đã được cập nhật";
                }
                else
                {
                    res.Status = StatusID.InternalServer;
                    res.Message = "Email không chính xác hoặc đã bị trùng";
                }

            }
            catch (Exception e)
            {
                res.Status = StatusID.InternalServer;
                res.Message = e.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DoanVien_Delete(int ID)
        {
            ResponseBase res = new ResponseBase();
            try
            {
                var datadangky = (from a in db.DangKies
                                  where a.IDDoanVien == ID
                                  select a).ToList();
                db.DangKies.DeleteAllOnSubmit(datadangky);
                db.SubmitChanges();
                var dt = (from a in db.DoanViens
                          where a.ID == ID
                          select a).FirstOrDefault();
                db.DoanViens.DeleteOnSubmit(dt);
                db.SubmitChanges();
                res.Status = StatusID.Success;
                res.Message = "Tất cả thông tin đoàn viên và lịch sử tham gia sự kiện của đoàn viên đã được xóa thành công";

            }
            catch (Exception e)
            {
                res.Status = StatusID.InternalServer;
                res.Message = e.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DoanVien_Export()
        {
            ExcelLayer ex = new ExcelLayer();
            try
            {

                var dt = (from a in db.DoanViens
                          orderby a.HoTen
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
                              MSSV = a.MSSV,
                              NoiVaoDoan = a.NoiVaoDoan,
                              TenXepLoai = getXepLoai(a.XepLoai),
                              XepLoai = a.XepLoai,
                              NhanThucChinhTri = a.NhanThucChinhTri,
                              RenLuyenCaNhan = a.RenLuyenCaNhan,
                              ChuyenMonNghiepVu = a.ChuyenMonNghiepVu
                          }).ToList();
                byte[] excelData = ex.ExportToExcel("DanhSach_DoanVien", dt);
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var stream = new MemoryStream(excelData);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "DanhSach_DoanVien.xlsx"
                };
                return File(excelData, System.Net.Mime.MediaTypeNames.Application.Octet, "DanhSach_DoanVien.xlsx");
            }
            catch (Exception)
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            }

        }
        public JsonResult GetUser()
        {
            UserSave datas = new UserSave();
            var ses = (UserSave)Session[Constant.USER_SESSION];
            if (ses == null)
            {
                datas.ID = 0;
            }
            else
            {
                datas.ID = ses.ID;
                datas.HoTen = ses.HoTen;
                datas.UserName = ses.UserName;
            }
            return Json(datas, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ImportExcel(string[] input)
        {
            ResponseBase res = new ResponseBase();
            StringBuilder builder = new StringBuilder();
            foreach (string value in input)
            {
                builder.Append(value);
            }
            var str = builder.ToString();
            var lst = JsonConvert.DeserializeObject<DoanVienImportModel>(str);
            if (lst.excelRows.Count > 0)
            {
                foreach (var m in lst.excelRows)
                {
                    if (Utils.checkEmailDoanvienAdd(m.Email) && Utils.IsValidEmail(m.Email))
                    {
                        Random generator = new Random();
                        String randomPass = generator.Next(0, 999999).ToString("D6");
                        DoanVien dt = new DoanVien
                        {
                            HoTen = m.HoTen,
                            DiaChi = m.DiaChi,
                            Email = m.Email,
                            GioiTinh = m.GioiTinh == "Nam",
                            NgaySinh = DateTime.Parse(m.NgaySinh),
                            NgayVaoDoan = DateTime.Parse(m.NgayVaoDoan),
                            Password = randomPass,
                            SoDienThoai = m.SoDienThoai,
                            NoiVaoDoan = m.NoiVaoDoan,
                            RenLuyenCaNhan = m.RenLuyenCaNhan,
                            NhanThucChinhTri = m.NhanThucChinhTri,
                            ChuyenMonNghiepVu = m.ChuyenMonNghiepVu,
                            MSSV = m.MSSV,
                            XepLoai = getXepLoai(m)
                        };
                        db.DoanViens.InsertOnSubmit(dt);
                        db.SubmitChanges();
                        string email = WebConfigurationManager.AppSettings["Email_Support"]; // Lấy email từ web.config
                        string password = WebConfigurationManager.AppSettings["PassWord_Email_Support"]; //Lấy mật khẩu từ web.config
                        var subject = "Thông báo tài khoản";
                        var body = "Hệ thống xin chào đoàn viên" + m.HoTen + ", mật khẩu của bạn là: " + randomPass;
                        var a = Utils.SendEmail(m.Email, subject, body, password, email);
                        if (a.Status == StatusID.Success)
                        {
                            res.Status = StatusID.Success;
                            res.Message = "Import và gửi email thành công!";
                        }
                        else
                        {
                            res.Status = StatusID.ServerBusy;
                            res.Message = "Lỗi phát sinh trong quá trình gửi mail thông báo. Vui lòng thử lại sau";
                            break;
                        }
                    }
                    else
                    {
                        res.Status = StatusID.ServerBusy;
                        res.Message = "Import thất bại do email của đoàn viên " + m.HoTen.Trim() + " không đúng định dạng hoặc bị trùng";
                        break;
                    }
                }

            }
            else
            {
                res.Status = StatusID.ServerBusy;
                res.Message = "Không tìm thấy đoàn viên, vui lòng kiểm tra lại ";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}