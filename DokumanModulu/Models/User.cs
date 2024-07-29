using Microsoft.AspNet.Identity.EntityFramework;

namespace DokumanModulu.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
    }
}
