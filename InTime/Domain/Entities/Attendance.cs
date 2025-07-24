namespace InTime.Domain.Entities
{
    public class Attendance
    {
        public Guid Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        public AttendanceStatus Status { get; set; } = AttendanceStatus.OnTime;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum AttendanceStatus
    {
        OnTime,
        Late,
        Absent
    }

}
