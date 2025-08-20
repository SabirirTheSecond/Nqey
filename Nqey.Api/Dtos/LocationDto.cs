namespace Nqey.Api.Dtos
{
    public class PositionDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Accuracy { get; set; }
    }
    public class LocationDto
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public PositionDto Position { get; set; }
        public string? City { get; set; }
        public string? Wilaya { get; set; }
    }
}
