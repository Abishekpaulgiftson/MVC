using mvc_1.Models.Domain;
using Microsoft.EntityFrameworkCore;
using mvc_1.Data;
using Microsoft.Identity.Client;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace mvc_1.Repositories
{
    public class CloudinaryImageRepository : IImageRepository
    {
        private readonly IConfiguration configuration;
        private readonly Account account;
        public CloudinaryImageRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            account = new Account(
                configuration.GetSection("Cloudinary")["MVC"],
                configuration.GetSection("Cloudinary")["473538336894399"],
                configuration.GetSection("Cloudinary")["jjW-p45lhEvWPjfKGesD9uukTGw"]);
        }
        public async Task<string> UploadAsync(IFormFile file)
        {
            var client = new Cloudinary(account);
            var uploadParams = new ImageUploadParams()
            {
                File=new FileDescription(file.FileName,file.OpenReadStream()),
                DisplayName=file.FileName
            };
            var uploadResult = await client.UploadAsync(uploadParams);
            if(uploadResult!=null && uploadResult.StatusCode==System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }
            return null;
        }
    }
}
