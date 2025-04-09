using Nqey.Domain;
namespace Nqey.Api.Dtos
{
    public class ServiceGetDto
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public List<Provider>? Providers { get; set; }
    }
}
