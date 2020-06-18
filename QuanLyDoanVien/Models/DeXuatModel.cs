using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyDoanVien.Models
{
    public class DeXuatModel
    {
        public int ID { get; set; }
        public int IDDoanVien { get; set; }
        public string TenDoanVien { get; set; }
        public string NoiDung { get; set; }
        public string NgayThang { get; set; }
        public int Loai { get; set; }
        public string TrangThai { get; set; }
        public string NguoiDeXuat { get; set; }
    }
}