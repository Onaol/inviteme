using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InviteMe.Models;

public class Event : Address
{
    public Guid Id { get; set; }
    
    [Required]
    public string TenantId { get; set; }

    [DisplayName("Event Name")][Required]
    public string Name { get; set; } = string.Empty;

    [DisplayName("Event Description")][Required]
    public string Description { get; set; } = string.Empty;

    [DisplayName("Event Date & Time")][Required]
    public DateTime EventDateTime { get; set; }

    public string? ImageUrl { get; set; }
    public Guid EventCategoryId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public EventCategory EventCategory { get; set;}

    public ICollection<Invite> Invites { get; set; }
}