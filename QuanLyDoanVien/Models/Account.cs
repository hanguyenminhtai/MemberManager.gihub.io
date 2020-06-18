using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyDoanVien.Models
{
    public class Account
    {
        public string Code { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string PasswordOld { get; set; }
        public string PasswordNew { get; set; }
        public string EmailAdress { get; set; }
    }
}