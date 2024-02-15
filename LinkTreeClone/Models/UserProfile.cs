using System.ComponentModel.DataAnnotations;

namespace LinkTreeClone.Models
{
    public class UserProfile
    {

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Age is required.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Instagram is required.")]
        public string InstagramAccount { get; set; }
       
        public int UserId { get; set; }
    }
}