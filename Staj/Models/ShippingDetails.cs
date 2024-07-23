using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Staj.Entity;

namespace Staj.Models
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Lütfen Kullanıcı İsmini Giriniz.")]
        public string Username { get; set; }
        
        [Required(ErrorMessage ="Lütfen Adres Tanımı Giriniz.")]
        public string AddressTitle { get; set; }
        
        [Required(ErrorMessage = "Lütfen Adresi Giriniz.")]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "Lütfen Şehri Giriniz.")]
        public string City { get; set; }
        
        [Required(ErrorMessage = "Lütfen İlçeyi Giriniz.")]
        public string District { get; set; }
        
        [Required(ErrorMessage = "Lütfen Mahalleyi Giriniz.")]
        public string Neighborhood { get; set; }
        public string PostCode { get; set; }
    }
}