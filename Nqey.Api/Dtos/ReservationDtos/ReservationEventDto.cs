namespace Nqey.Api.Dtos.ReservationDtos
{
    public class ReservationEventDto
    {
        public int ReservationEventId { get; set; }
        public string ReservationEventType { get; set; }  // mapped from enum as string
        public DateTime CreatedAt { get; set; }
        public string? Notes { get; set; }
    }
}
