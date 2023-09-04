using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InviteMe.Models.ViewModels;

public class EventViewModel : Address
{
    public Guid Id { get; set; }
    
    [DisplayName("Event Name")][Required(ErrorMessage = "Event is required")]
    public string Name { get; set; } = string.Empty;

    [DisplayName("Event Description")][Required(ErrorMessage = "Event description is required")]
    public string Description { get; set; } = string.Empty;

    [DisplayName("Event Date & Time")]
    [Required]
    public DateTime EventDateTime { get; set; }

    [DisplayName("Banner Image")]
    [Required(ErrorMessage = "Banner Image is required")]
    public IFormFile EventImage { get; set; }
    public Guid EventCategoryId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public EventCategory EventCategory { get; set;}
}