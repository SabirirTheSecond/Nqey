namespace Nqey.Api.Dtos.RecoDtos
{
    public class ProviderSendApiDto
    {
        public int Id { get; set; }
        public int? Service_Id { get; set; }
        public List<int> Subservices { get; set; } = new();
        public double Avg_Rating { get; set; }
        public int Jobs_Done { get; set; }

    }
}
