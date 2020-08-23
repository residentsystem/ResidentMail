using System.ComponentModel.DataAnnotations;

namespace ResidentMail.Models
{
    public class Contact
    {
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 5)]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
    }
}