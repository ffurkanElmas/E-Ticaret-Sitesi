using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DokumanModulu.Models
{
    public class Register
    {
        [DisplayName("İsim")]
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [DisplayName("Soyisim")]
        [Required]
        [StringLength(128)]
        public string Surname { get; set; }

        [DisplayName("Telefon Numarası")]
        [Required]
        [StringLength(10, ErrorMessage = "Telefon numarası 10 karakter olmalıdır.")]
        public string PhoneNumber { get; set; }

        [DisplayName("E-Mail Adresi")]
        [Required]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-mail adresi giriniz.")]
        [StringLength(128)]
        public string Email { get; set; }

        [DisplayName("Şifre")]
        [Required]
        public string Password { get; set; }

        [DisplayName("Şifre Tekrar")]
        [Required]
        [Compare("Password", ErrorMessage = "Şifre uyuşmuyor!")]
        public string RePassword { get; set; }

        [DisplayName("Rol")]
        [Required]
        public string Role { get; set; }
    }
}
