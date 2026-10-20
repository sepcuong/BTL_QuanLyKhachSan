using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helper
{
    public class AppSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = "BTL_QuanLyKhachSan";
        public string Audience { get; set; } = "BTL_QuanLyKhachSan.Client";
    }
}
