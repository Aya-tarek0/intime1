namespace InTime.Domain.Entities
{
    public class FirebaseDevice
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Platform { get; set; } = "Web";

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
    }

}
