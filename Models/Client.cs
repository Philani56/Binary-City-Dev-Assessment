using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Client
    {
        // Primary key for the Client entity
        [Key]
        public int Id { get; set; }

        // Required property representing the client's name
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        // Optional property representing a unique client code
        // Must be exactly 6 characters long
        [Display(Name = "Client Code")]
        [StringLength(6, MinimumLength = 6)] // Ensures the client code is exactly 6 characters
        public string? ClientCode { get; set; }

        // Foreign key to link to the Contact entity
        [Display(Name = "Link Contacts")]
        public int ContactId { get; set; } // Foreign key to associate with a Contact

        // Navigation property to the related Contact entity
        public Contact Contact { get; set; } // Navigation property to access the related Contact
    }
}
