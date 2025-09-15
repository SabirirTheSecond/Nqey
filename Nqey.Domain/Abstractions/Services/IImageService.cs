using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Nqey.Domain.Common;

namespace Nqey.Domain.Abstractions.Services
{
    public interface IImageService
    {
        Task<ProfileImage?> UploadImageSafe(IFormFile file, int userId, string errorMessage= "Image Upload Failed");
        Task<List<PortfolioImage>> UploadPortfolioImages(IEnumerable<IFormFile>? files, int providerId);
        Task<Image?> UploadServiceImage(IFormFile file, int serviceId);
        Task<List<ComplaintAttachement>> UploadAttachmentImages(IEnumerable<IFormFile>? files, int complaintId);
    }
}
