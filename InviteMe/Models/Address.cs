using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InviteMe.Models
{
    public class Address
    {
        [DisplayName("Address First Line")]
        [Required]
        public string AddressFirstLine { get; set; } = string.Empty;

        [DisplayName("Address Second Line")]
        public string? AddressSecondLine { get; set; }

        [DisplayName("Address Third Line")]
        public string? AddressThirdLine { get; set; }

        [DisplayName("Post Code")]        
        public string? PostCode { get; set; } = string.Empty;

        [DisplayName("Map Link")]
        public string? MapUrl { get; set; }
    }
}