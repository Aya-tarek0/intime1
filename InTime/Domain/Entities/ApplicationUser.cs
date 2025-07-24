namespace InTime.Domain.Entities
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<FirebaseDevice> Devices { get; set; } = new List<FirebaseDevice>();
        public GoogleCalendarIntegration? CalendarIntegration { get; set; }
    }

}
