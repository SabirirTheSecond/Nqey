using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Nqey.Domain.Abstractions.Services
{
    public interface IImageUploaderService
    {
        Task<string> UploadImageToSupabase(IFormFile file);
    }
}
