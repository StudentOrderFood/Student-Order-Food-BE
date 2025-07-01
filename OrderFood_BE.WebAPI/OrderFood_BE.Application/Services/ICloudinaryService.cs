using Microsoft.AspNetCore.Http;

namespace OrderFood_BE.Application.Services
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file, string folder);
    }
}
