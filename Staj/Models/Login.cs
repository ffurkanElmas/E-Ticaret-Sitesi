using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Staj.Models
{
    public class Login
    {
        [DisplayName("Kullanıcı Adı")]
        [Required]
        public string Username { get; set; }

        [DisplayName("Şifre")]
        [Required]
        public string Password { get; set; }

        [DisplayName("Beni Hatırla")]
        public Boolean RememberMe { get; set; }
    }
}