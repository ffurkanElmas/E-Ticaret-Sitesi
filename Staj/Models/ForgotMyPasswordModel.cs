using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Staj.Models
{
    public class ForgotMyPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}