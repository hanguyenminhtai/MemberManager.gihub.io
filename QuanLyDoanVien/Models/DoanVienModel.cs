using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyDoanVien.Models
{
    public class DoanVienModel
    {
        public int ID { get; set; }
        public string HoTen { get; set; }
        public string NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string NgayVaoDoan { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string NoiVaoDoan { get; set; }
        public string MSSV { get; set; }
        public int XepLoai { get; set; }
        public string TenXepLoai { get; set; }
        public decimal? NhanThucChinhTri { get; set; }
        public decimal? ChuyenMonNghiepVu { get; set; }
        public decimal? RenLuyenCaNhan { get; set; }
        public bool? LaBiThu { get; set; }
    }
    public class DoanVienImportModel
    {
        public List<DoanVienModel> excelRows { get; set; }

    }
}