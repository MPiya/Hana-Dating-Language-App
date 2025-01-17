using Microsoft.AspNetCore.Identity;

namespace Hana.Models
{
    // This class represents the AspNetUsers identity part
    public class UserIdentity : IdentityUser
    {
        // Navigation property to UserProfile
        public virtual UserProfile Profile { get; set; }
    }
} 