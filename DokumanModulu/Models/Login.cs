using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DokumanModulu.Models
{
    public class Login
    {
        [DisplayName("E-Mail Adresi")]
        [Required]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-mail adresi giriniz.")]
        public string Email { get; set; }

        [DisplayName("Şifre")]
        [Required]
        public string Password { get; set; }
    }
}
