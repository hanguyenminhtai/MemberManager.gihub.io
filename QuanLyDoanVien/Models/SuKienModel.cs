using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyDoanVien.Models
{
    public class SuKienModel
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public string NoiDung { get; set; }
        public string DiaDiem { get; set; }
        public string BatDau { get; set; }
        public string GhiChu { get; set; }
    }
    public class SuKienDoanVienModel 
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public string NoiDung { get; set; }
        public string DiaDiem { get; set; }
        public string BatDau { get; set; }
        public string GhiChu { get; set; }
        public int TrangThai { get; set; }
        public string TenTrangThai { get; set; }
    }
}