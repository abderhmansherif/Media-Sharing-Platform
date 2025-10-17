using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using BeatBox.Models;

namespace BeatBox.Services
{
    public interface IMediaManager
    {
        bool ImageValidationFilter(IFormFile file);
        bool VideoValidationFilter(IFormFile file);
        bool AudioValidationFilter(IFormFile file);
        long GetMediaSize(IFormFile file);

        Task<MediaOperationResult> UploadImageAsync(IFormFile file);
        Task<MediaOperationResult> UploadVideoAsync(IFormFile file);
        Task<MediaOperationResult> UploadAudioAsync(IFormFile file);

        Task<MediaOperationResult> UploadImageToServerAsync(IFormFile file);
        Task<MediaOperationResult> UploadAudioToServerAsync(IFormFile file);
        Task<MediaOperationResult> UploadVideoToServerAsync(IFormFile file);

        Task<MediaOperationResult> SaveFileToServerAsync(IFormFile file, string folder);


        Task<MediaOperationResult> UpdateFileAsync(IFormFile file, string publicId, CloudinaryDotNet.Actions.ResourceType resourceType);

        Task<MediaOperationResult> RemoveFileAsync(string publicId);
    }
}