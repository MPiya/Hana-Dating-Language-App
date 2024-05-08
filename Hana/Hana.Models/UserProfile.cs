using Microsoft.AspNetCore.Identity;

namespace Hana.Models
{
    public class UserProfile  : IdentityUser
    {
       
        public string Name { get; set; }
    }
}