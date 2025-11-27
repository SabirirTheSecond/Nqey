namespace Nqey.Services.Services
{
    public class ProviderRecommenderRequestDto
    {
        public List<ProviderSendDto> Providers { get; set; } = new();
        public ClientHistorySendDto Client_History { get; set; } = new();
        public MetaSendDto Meta { get; set; } = new();
    }
}
