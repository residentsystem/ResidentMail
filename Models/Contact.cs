using System.ComponentModel.DataAnnotations;

namespace ResidentMail.Models
{
    public class Contact
    {
        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(60)]
        public string Email { get; set; }

        [Required]
        [StringLength(60)]
        public string Subject { get; set; }

        [Required]
        [StringLength(1000)]
        public string Message { get; set; }

        [Required]
        [StringLength(2), MinLength(1)]
        public string Sum { get; set; }

        public string Honey { get; set; }
    }
}