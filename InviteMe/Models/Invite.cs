namespace InviteMe.Models
{
    public class Invite : Tenant
    {
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public Guid EventId { get; set; }
        public string UniqueInviteKey { get; set; }
        public bool Sent { get; set; }
        public bool Acknowledged { get; set; }
        public bool Attended { get; set; }
        public DateTime TimeCheckedIn { get; set; }

        public Contact Contact { get; set; }
        public Event Event { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime TimeUpdated { get; set; } = DateTime.UtcNow;       
    }
}
