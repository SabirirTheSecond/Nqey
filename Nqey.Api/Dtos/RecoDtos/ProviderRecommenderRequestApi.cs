namespace Nqey.Api.Dtos.RecoDtos
{
   
        public class ProviderRecommendRequestApi
        {
            public List<ProviderSendApiDto> Providers { get; set; } = new();
            public ClientHistorySendApiDto Client_History { get; set; } = new();
            public MetaSendApiDto Meta { get; set; } = new();
        }
    
}
