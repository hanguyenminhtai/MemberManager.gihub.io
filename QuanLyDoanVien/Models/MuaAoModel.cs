using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyDoanVien.Models
{
    public class MuaAoModel
    {
        public int ID { get; set; }
        public int IDDoanVien { get; set; }
        public string HoTen { get; set; }
        public int SoLuong { get; set; }
        public string NoiDung { get; set; }
        public string NgayThang { get; set; }
        public int TrangThai { get; set; }
    }
}