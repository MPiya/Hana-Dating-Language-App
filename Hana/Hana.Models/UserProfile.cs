using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hana.Models
{
    [Table("UserProfiles")]
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }  // Primary key for UserProfile table

        public string UserId { get; set; }  // Foreign key to AspNetUsers table
        
        [StringLength(10)]
        public string? Name { get; set; }
        
      
        public int? Age { get; set; }

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
        public string? Nationality { get; set; }

        [ForeignKey("UserId")]
        public virtual UserIdentity User { get; set; }  // Navigation property

        // New property to store image URLs
        public List<string> ImageUrls { get; set; } = new List<string>();
    }
}
