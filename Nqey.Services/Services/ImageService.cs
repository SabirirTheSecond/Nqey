using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Nqey.Domain.Abstractions.Services;
using Nqey.Domain.Common;


namespace Nqey.Services.Services
{
    public class ImageService: IImageService
    {
        private readonly IImageUploaderService _imageUploaderService;

        public ImageService(IImageUploaderService imageUploaderService)
        {
            _imageUploaderService = imageUploaderService;
        }

        public async Task<List<ComplaintAttachement>> UploadAttachmentImages(IEnumerable<IFormFile>? files, int complaintId)
        {
            var attachments = new List<ComplaintAttachement>();

            if (files != null && files.Any())
            {
                foreach (var file in files)
                {
                    var imagePath = await _imageUploaderService.UploadImageToSupabase(file);
                    attachments.Add(new ComplaintAttachement
                    {
                        FileUrl = imagePath,
                        ComplaintId = complaintId
                    });
                }
            }
            return attachments;
        }

        public async Task<ProfileImage?> UploadImageSafe(IFormFile file,int userId, string errorMessage = "Image Upload Failed")
        {
            string? imagePath = null;
           ProfileImage? image= new ProfileImage();
            try
            {
                imagePath= await _imageUploaderService.UploadImageToSupabase(file);
               
                if (imagePath != null)
                {

                    image= new ProfileImage
                    {

                        ImagePath = imagePath,
                        UserId = userId

                    };


                }
                return image;

            }
            catch (Exception ex)
            {
                throw new Exception($"{errorMessage}: {ex.Message}");
            }
        }
         

          

        

        public async Task<List<PortfolioImage>> UploadPortfolioImages(IEnumerable<IFormFile>? files,
            int providerId)
        {
            var images = new List<PortfolioImage>();
            //Uploading Portfolio images:
            if (files != null && files.Any())
            {
                
                foreach (var file in files)
                {
                    
                       var imagePath = await _imageUploaderService.UploadImageToSupabase(file);
                        images.Add(new PortfolioImage
                        {
                            ImagePath = imagePath,
                            ProviderUserId = providerId,
                        });
                }
                return images;
            }
            else { return images; }



            
        }

        public async Task<Image?> UploadServiceImage(IFormFile file, int serviceId)
        {

            string? imagePath = null;
            var serviceImage = new Image();

            
                try
                {
                    imagePath = await _imageUploaderService.UploadImageToSupabase(file);
                    if(imagePath != null)
                    {
                         serviceImage = new Image
                        {
                            ImagePath = imagePath
                        };
                           
                            
                    }
                    return serviceImage;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error: {ex.Message}");
                }
            

            
        }
    }
}
