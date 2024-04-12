using Hana.Hana.Database.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Hana.Models
{
    public class UserProfile : IdentityUser
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(10)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [StringLength(10)]
        public string Age { get; set; }

        [StringLength(10)]
        public string InstagramAccount { get; set; }

        [Required(ErrorMessage = "Bio is required")]
        [StringLength(50)]
        public string Bio { get; set; }

        [Required(ErrorMessage = "Interest is required")]
        [StringLength(50)]
        public string Interest { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(30)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Speak Language is required")]
        [StringLength(100)]
        public string SpeakLanguage { get; set; }

        [Required(ErrorMessage = "Learn Language is required")]
        [StringLength(100)]
        public string LearnLanguage { get; set; }
    }
}
