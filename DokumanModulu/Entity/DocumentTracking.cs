using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DokumanModulu.Identity;

namespace DokumanModulu.Entity
{
    public class DocumentTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        [DisplayName("Açıklama")]
        public string Description { get; set; }

        
        [StringLength(256)]
        [DisplayName("Dosya Yolu")]
        public string Path { get; set; }

        [DisplayName("Oluşturulma Tarihi")]
        public DateTime? CreatedDate { get; set; }

        [DisplayName("Oluşturan Kişi ID'si")]
        [Required]
        public string CreatedUserId { get; set; }

        public string AllowedUsers { get; set; }

    }
}
