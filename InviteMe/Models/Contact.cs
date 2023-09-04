using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InviteMe.Models
{
    public class Contact:Tenant
    {
        [Key]
        public Guid Id { get; set; }      

        [DisplayName("First Name")][Required] 
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; } = string.Empty;
        [Required][DisplayName("Email Address")] 
        public string Email { get; set; } = string.Empty;
        
        [DisplayName("Phone Number")]
        public string? PhoneNumber { get; set; } = string.Empty;        
    }
}
