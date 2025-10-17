using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatBox.Models;
using Microsoft.EntityFrameworkCore;

namespace BeatBox.Services
{
    public class FileManager : IFileManager
    {
        private readonly AppDbContext _context;

        public FileManager(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FileOperationResults<string>> FindPublicIdByUrlAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return new FileOperationResults<string>
                {
                    Result = false,
                    Errors = new List<string> { "Invalid Url" }
                };

            var uploadRecord = await _context.cloudinaryUploads.FirstOrDefaultAsync(x => x.Url == url);

            if (uploadRecord == null)
                return new FileOperationResults<string>
                {
                    Result = false,
                    Errors = new List<string> { "Url does not exist" }
                };

            return new FileOperationResults<string>
            {
                Result = true,
                Output = uploadRecord.PublicId
            };
        }

        public async Task<FileOperationResults<string>> FindUrlByPublicIdAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                return new FileOperationResults<string>
                {
                    Result = false,
                    Errors = new List<string> { "Invalid PublicId" }
                };

            var uploadRecord = await _context.cloudinaryUploads.FirstOrDefaultAsync(x => x.PublicId == publicId);

            if (uploadRecord == null)
                return new FileOperationResults<string>
                {
                    Result = false,
                    Errors = new List<string> { "PublicId does not exist" }
                };

            return new FileOperationResults<string>
            {
                Result = true,
                Output = uploadRecord.Url
            };
        }

        public async Task<FileOperationResults<bool>> CreateCloudinaryUploadAsync(CloudinaryUploads cloudinaryUpload)
        {
            if (cloudinaryUpload == null)
                return new FileOperationResults<bool>
                {
                    Result = false,
                    Errors = new List<string> { "Invalid parameters" }
                };

            var existing = await _context.cloudinaryUploads.FirstOrDefaultAsync(
                x => x.Url == cloudinaryUpload.Url && x.PublicId == cloudinaryUpload.PublicId);

            if (existing != null)
                return new FileOperationResults<bool>
                {
                    Result = false,
                    Errors = new List<string> { "Upload record already exists" }
                };

            await _context.cloudinaryUploads.AddAsync(cloudinaryUpload);
            await _context.SaveChangesAsync();

            return new FileOperationResults<bool>
            {
                Result = true,
                Output = true
            };
        }

        public async Task<FileOperationResults<bool>> RemoveCloudinaryUploadByPublicIdAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                return new FileOperationResults<bool>
                {
                    Result = false,
                    Errors = new List<string> { "Invalid PublicId" }
                };

            var uploadRecord = await _context.cloudinaryUploads.FirstOrDefaultAsync(x => x.PublicId == publicId);

            if (uploadRecord == null)
                return new FileOperationResults<bool>
                {
                    Result = false,
                    Errors = new List<string> { "PublicId does not exist" }
                };

            _context.cloudinaryUploads.Remove(uploadRecord);
            await _context.SaveChangesAsync();

            return new FileOperationResults<bool>
            {
                Result = true,
                Output = true
            };
        }

        public async Task<FileOperationResults<bool>> RemoveCloudinaryUploadByUrlAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return new FileOperationResults<bool>
                {
                    Result = false,
                    Errors = new List<string> { "Invalid Url" }
                };

            var uploadRecord = await _context.cloudinaryUploads.FirstOrDefaultAsync(x => x.Url == url);

            if (uploadRecord == null)
                return new FileOperationResults<bool>
                {
                    Result = false,
                    Errors = new List<string> { "Url does not exist" }
                };

            _context.cloudinaryUploads.Remove(uploadRecord);
            await _context.SaveChangesAsync();

            return new FileOperationResults<bool>
            {
                Result = true,
                Output = true
            };
        }

        public async Task<FileOperationResults<bool>> UpdateCloudinaryUploadAsync(string publicId, string newUrl)
        {
            if (string.IsNullOrWhiteSpace(publicId) || string.IsNullOrWhiteSpace(newUrl))
                return new FileOperationResults<bool>
                {
                    Result = false,
                    Errors = new List<string> { "Invalid parameters" }
                };

            var uploadRecord = await _context.cloudinaryUploads.FirstOrDefaultAsync(x => x.PublicId == publicId);

            if (uploadRecord == null)
                return new FileOperationResults<bool>
                {
                    Result = false,
                    Errors = new List<string> { "PublicId does not exist" }
                };

            uploadRecord.Url = newUrl;
            _context.cloudinaryUploads.Update(uploadRecord);
            await _context.SaveChangesAsync();

            return new FileOperationResults<bool>
            {
                Result = true,
                Output = true
            };
        }
    }
}
