using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyDoanVien.Models
{
    public class ResponseBase
    {
        public StatusID Status { get; set; }
        public string Message { get; set; }
    }
    public enum StatusID
    {
        Success = 1,
        InternalServer,
        TokenInvalid,
        IDNotFound,
        BadRequest,
        ServerBusy,
        AccessDenied
    }
    public class BenhNhanModel
    {
        public int ID { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string GhiChu { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class BacSiModel
    {
        public int ID { get; set; }
        public string HoTen { get; set; }
        public int IDKhoa { get; set; }
        public string TenKhoa { get; set; }
        public string GioiTinh { get; set; }
        public string NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class KhoaModel
    {
        public int ID { get; set; }
        public string TenKhoa { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
    }
}