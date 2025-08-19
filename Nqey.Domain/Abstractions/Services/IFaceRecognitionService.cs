using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Abstractions.Services
{
    public interface IFaceRecognitionService

    {
        Task<bool> VerifyFacesAsync(string idImageUrl, string selfieImageUrl);
    }
}
