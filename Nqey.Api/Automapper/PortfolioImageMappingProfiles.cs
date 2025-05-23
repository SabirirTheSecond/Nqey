using AutoMapper;
using Nqey.Api.Dtos;
using Nqey.Domain.Common;

namespace Nqey.Api.Automapper
{
    public class PortfolioImageMappingProfiles : Profile
    {
        public PortfolioImageMappingProfiles() {


            CreateMap<PortfolioImage, PortfolioImageDto>();


        }
        
    }
}
