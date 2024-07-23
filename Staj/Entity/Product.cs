using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Staj.Entity
{
    public class Product
    {
        public int Id { get; set; }

        [DisplayName("Ürün Adı")]
        public string Name { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }

        [DisplayName("Fiyat")]
        public double Price { get; set; }

        [DisplayName("Stok Miktarı")]
        public int Stock { get; set; }

        [DisplayName("Satışta mı?")]
        public bool IsApproved { get; set; }

        [DisplayName("Ana Sayfada mı?")]
        public bool IsHome { get; set; }

        [DisplayName("Kategori ID'si")]
        public int CategoryId { get; set; }

        [DisplayName("Görsel")]
        public string Image { get; set; }
        public Category Category { get; set; }
        
    }
}