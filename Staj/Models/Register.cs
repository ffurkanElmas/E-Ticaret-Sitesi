using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Staj.Models
{
    public class Register
    {
        [DisplayName("İsim")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Soyisim")]
        [Required]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı Adı")]
        [Required]
        public string Username { get; set; }

        [DisplayName("E-Mail Adresi")]
        [Required]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-mail adresi giriniz.")]
        public string Email { get; set; }

        [DisplayName("Şifre")]
        [Required]
        public string Password { get; set; }

        [DisplayName("Şifre Tekrar")]
        [Required]
        [Compare("Password" , ErrorMessage = "Şifre uyuşmuyor!")]
        public string RePassword { get; set; }
    }
}