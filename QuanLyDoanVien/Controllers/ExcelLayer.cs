using OfficeOpenXml;
using OfficeOpenXml.Style;
using QuanLyDoanVien.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace QuanLyDoanVien.Controllers
{
    public class ExcelLayer
    {
        public byte[] ExportToExcel(string FileName, List<DoanVienModel> m)
        {
            try
            {
                FileInfo newFile = new FileInfo(FileName);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(FileName);
                }
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Danh sách đoàn viên");
                    //worksheet.Protection.IsProtected = true;
                    //worksheet.Protection.SetPassword("hieupv@thienan.vn");
                    worksheet.Column(1).Width = 5;
                    worksheet.Column(2).Width = 5;
                    worksheet.Column(3).Width = 34;
                    worksheet.Column(4).Width = 16;
                    worksheet.Column(5).Width = 14;
                    worksheet.Column(6).Width = 16;
                    worksheet.Column(7).Width = 16;
                    worksheet.Column(8).Width = 35;
                    worksheet.Column(9).Width = 35;
                    using (var tieude = worksheet.Cells[1, 1, 1, 9])
                    {
                        tieude.Merge = true;
                        tieude.Value = "danh sách đoàn viên".ToUpper();
                        tieude.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        tieude.Style.Font.Bold = true;
                        tieude.Style.Font.SetFromFont(new Font("Times New Roman", 16));
                    }
                    worksheet.Row(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Row(2).Style.Font.SetFromFont(new Font("Times New Roman", 14));
                    worksheet.Cells[2, 1].Value = "STT";
                    worksheet.Cells[2, 2].Value = "MSSV";
                    worksheet.Cells[2, 3].Value = "Họ tên";
                    worksheet.Cells[2, 4].Value = "Ngày sinh";
                    worksheet.Cells[2, 5].Value = "Giới tính";
                    worksheet.Cells[2, 6].Value = "Ngày vào đoàn";
                    worksheet.Cells[2, 7].Value = "Điện thoại";
                    worksheet.Cells[2, 8].Value = "Email";
                    worksheet.Cells[2, 9].Value = "Địa chỉ";
                    worksheet.Cells[2, 10].Value = "Nơi vào đoàn";
                    worksheet.Cells[2, 11].Value = "Xếp loại";
                    int stt = 1;
                    int rowindex = 3;
                    foreach (var i in m)
                    {
                       worksheet.Row(rowindex).Style.Font.SetFromFont(new Font("Times New Roman", 13));
                        worksheet.Cells[rowindex, 1].Value = stt;
                        worksheet.Cells[rowindex, 2].Value = i.MSSV;
                        worksheet.Cells[rowindex, 3].Value = i.HoTen;
                        worksheet.Cells[rowindex, 4].Value = i.NgaySinh;
                        worksheet.Cells[rowindex, 5].Value = i.GioiTinh;
                        worksheet.Cells[rowindex, 6].Value = i.NgayVaoDoan;
                        worksheet.Cells[rowindex, 7].Value = i.SoDienThoai;
                        worksheet.Cells[rowindex, 8].Value = i.Email;
                        worksheet.Cells[rowindex, 9].Value = i.DiaChi;
                        worksheet.Cells[rowindex, 10].Value = i.NoiVaoDoan;
                        worksheet.Cells[rowindex, 11].Value = i.TenXepLoai;
                        stt -= -1;
                        rowindex -= -1;
                    }

                    using (var baocao = worksheet.Cells[2, 1, rowindex-1, 12])
                    {
                        try
                        {
                            baocao.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            baocao.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            baocao.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            baocao.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                    //worksheet.View.FreezePanes(4, 2);
                    //package.Save();     
                    return package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                string mes = m == null ? "DataIsNull" : "CountData = " + m.Count();
                return ExportError(ex, mes);
            }
        }
        public byte[] ExportError(Exception ex, string mes)
        {
            FileInfo newFile = new FileInfo("ExportError");
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo("ExportError");
            }
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Dữ liệu");
                worksheet.Protection.IsProtected = true;
                worksheet.Row(1).Height = 100;
                worksheet.Column(1).Width = 100;
                using (var tieu_de = worksheet.Cells[1, 1])
                {
                    tieu_de.Value = mes + ex.ToString();
                    tieu_de.Merge = true;
                    tieu_de.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    tieu_de.Style.Font.Name = "Times New Roman";
                    tieu_de.Style.Font.Size = 12;
                }
                //worksheet.View.FreezePanes(4, 2);
                //package.Save();     
                return package.GetAsByteArray();
            }
        }
    }
}