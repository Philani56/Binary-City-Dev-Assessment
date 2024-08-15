using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Contact
    {
        // Primary key for the Contact entity
        [Key]
        public int Id { get; set; }

        // Required property representing the contact's name
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        // Required property representing the contact's surname
        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        // Required property representing the contact's email address
        [Required]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")] // Validation for a valid email format
        [ValidEmailDomain("gmail.com", "co.za", ErrorMessage = "Email address must be @gmail.com or @co.za.")] // Custom validation for specific email domains
        public string Email { get; set; }

        // Navigation property for related Client entities; Not validated by model binding
        [ValidateNever]
        public ICollection<Client> Clients { get; set; } // A Contact can be linked to multiple clients 
    }
}
