using Microsoft.Build.Framework;
using System.ComponentModel;

namespace InviteMe.Models
{
    public class EventCategory : Tenant
    {
        public Guid Id { get; set; }

        [DisplayName("Event Category Name")][Required]        
        public string Name { get; set; } = string.Empty;

        [DisplayName("Event Category Description")]
        public string? Description { get; set; }
    }
}
