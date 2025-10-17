using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatBox.Models;

namespace BeatBox.Services
{
    public interface IFileManager
    {
        Task<FileOperationResults<string>> FindPublicIdByUrlAsync(string url);
        Task<FileOperationResults<string>> FindUrlByPublicIdAsync(string publicId);
        Task<FileOperationResults<bool>> CreateCloudinaryUploadAsync(CloudinaryUploads cloudinaryUpload);
        Task<FileOperationResults<bool>> RemoveCloudinaryUploadByUrlAsync(string url);
        Task<FileOperationResults<bool>> RemoveCloudinaryUploadByPublicIdAsync(string publicId);
        Task<FileOperationResults<bool>> UpdateCloudinaryUploadAsync(string publicId, string newUrl);
    }
}