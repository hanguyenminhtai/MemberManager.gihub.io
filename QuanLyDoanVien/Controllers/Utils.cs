using QuanLyDoanVien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace QuanLyDoanVien.Controllers
{
    public static class Utils
    {
        public static ResponseBase SendEmail(string to_email, string subject, string body, string password, string from_email)
        {
            ResponseBase res = new ResponseBase();
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.UseDefaultCredentials = false;
            mail.From = new MailAddress(from_email);
            mail.To.Add(to_email);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(from_email, password);
            try
            {
                // Cần cho phép các ứng dụng kém an toàn truy cập vào tài khoản email
                // Cho phép tại: https://myaccount.google.com/lesssecureapps
                SmtpServer.Send(mail);
                res.Status = StatusID.Success;
            }

            catch (Exception ex)
            {
                res.Status = StatusID.InternalServer;
                res.Message = ex.Message;
            }
            return res;
        }
        public static ResponseBase SendEmailBCC( string subject, string body, string password, string from_email,List<DoanVien> dt)
        {
            ResponseBase res = new ResponseBase();
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.UseDefaultCredentials = false;
            mail.From = new MailAddress(from_email);
            mail.To.Add(dt.FirstOrDefault().Email);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            foreach(var i in dt)
            {
                if (IsValidEmail(i.Email.Trim()))
                {
                    mail.CC.Add(i.Email.Trim());
                }
            }
            SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(from_email, password);
            try
            {
                // Cần cho phép các ứng dụng kém an toàn truy cập vào tài khoản email
                // Cho phép tại: https://myaccount.google.com/lesssecureapps
                SmtpServer.Send(mail);
                res.Status = StatusID.Success;
            }

            catch (Exception ex)
            {
                res.Status = StatusID.InternalServer;
                res.Message = ex.Message;
            }
            return res;
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool checkEmailDoanvien(int ID,string email)
        {
           
            LinqDataContext db = new LinqDataContext();
            var dt = (from a in db.DoanViens
                      where a.ID != ID && a.Email.Trim()==email
                      select a).Count();
            return dt == 0;
        }
        public static bool checkEmailDoanvienAdd( string email)
        {

            LinqDataContext db = new LinqDataContext();
            var dt = (from a in db.DoanViens
                      where  a.Email.Trim() == email
                      select a).Count();
            return dt == 0;
        }
    }
}