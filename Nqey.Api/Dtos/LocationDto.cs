namespace Nqey.Api.Dtos
{
    public class PositionDto
    {
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public int? Accuracy { get; set; }
    }
    public class LocationDto
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public PositionDto Position { get; set; }
        public string Ville { get; set; }
        public string Wilaya { get; set; }
    }
}
