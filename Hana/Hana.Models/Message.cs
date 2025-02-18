using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hana.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; }

        [Required]
        [StringLength(50)]
        public string SocialMediaPlatform { get; set; }

        [Required]
        [StringLength(255)]
        public string SocialMediaLink { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("SenderId")]
        public virtual UserIdentity Sender { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual UserIdentity Receiver { get; set; }
    }
} 