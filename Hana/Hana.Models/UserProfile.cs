using Hana.Hana.Database.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Hana.Models
{
    public class UserProfile : IdentityUser
    {
        
        [StringLength(10)]
        public string? Name { get; set; }

        
        [StringLength(10)]
        public string? Age { get; set; }

        [StringLength(10)]
        public string? InstagramAccount { get; set; }

     
        [StringLength(500)]
        public string? Bio { get; set; }

       
        [StringLength(50)]
        public string? Interest { get; set; }

       
        [StringLength(30)]
        public string? Gender { get; set; }

     
        [StringLength(100)]
        public string? SpeakLanguage { get; set; }

       
        [StringLength(100)]
        public string? LearnLanguage { get; set; }


       
        [StringLength(100)]
        public string?  Nationality { get; set; }
    }
}
