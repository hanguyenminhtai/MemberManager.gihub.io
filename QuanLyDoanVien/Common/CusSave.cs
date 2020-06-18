using QuanLyDoanVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyDoanVien.Common
{
    public class DoanVienSave :DoanVienModel
    {
    }
    public class UserSave
    {
        public int ID { set; get; }
        public string HoTen { set; get; }
        public string SoDienThoai { set; get; }
        public string DiaChi { set; get; }
        public string Email { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
       
    }
}