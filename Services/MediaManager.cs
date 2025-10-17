using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatBox.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;

namespace BeatBox.Services
{
    public class MediaManager : IMediaManager
    {
        private readonly Cloudinary _cloudinary;
        private readonly AppDbContext _context;
        private IWebHostEnvironment _webHostEnvironment;

        public MediaManager(Cloudinary cloudinary, AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _cloudinary = cloudinary;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public bool VideoValidationFilter(IFormFile file)
        {
            if (file == null)
                return false;

            if (file.Length == 0)
                return false;

            if (!file.ContentType.StartsWith("video/"))
                return false;

            var allowedExtensions = new List<string>() { ".mp4", ".mov", ".avi", ".mkv"};

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName)))
                return false;

            var LengthWithMB = (file.Length / 1024) / 1024;

            // if (LengthWithMB >= 25)
            //     return false;

            return true;
        }

        public bool AudioValidationFilter(IFormFile file)
        {
            if (file == null)
                return false;

            if (file.Length == 0)
                return false;

            if (!file.ContentType.StartsWith("audio/"))
                return false;

            var allowedExtensions = new List<string>() { ".wav", ".mp4", ".mpeg", ".mp3", ".m4a" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName)))
                return false;

            var LengthWithMB = (file.Length / 1024) / 1024;

            if (LengthWithMB >= 25)
                return false;

            return true;
        }

        public long GetMediaSize(IFormFile file)
        {
            if (file.Length == 0)
                return 0;

            long sizeInMB = (file.Length / 1024) / 1024;

            return sizeInMB;
        }

        public bool ImageValidationFilter(IFormFile file)
        {
            if (file == null)
                return false;

            if (file.Length == 0)
                return false;

            if (!file.ContentType.StartsWith("image/"))
                return false;

            var allowedExtensions = new List<string>() { ".png", ".jpg", ".jpeg" , ".webp", ".avif"};

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName)))
                return false;

            var LengthWithMB = (file.Length / 1024) / 1024;

            if (LengthWithMB >= 10)
                return false;

            return true;
        }

        public async Task<MediaOperationResult> UploadImageAsync(IFormFile file)
        {
            var isValidFile = ImageValidationFilter(file);

            if (!isValidFile)
                return new MediaOperationResult()
                {
                    Errors = new List<string>() { "Invalid file" },
                    result = false
                };

            var publicId = Guid.NewGuid().ToString();

            var param = new ImageUploadParams()
            {
                PublicId = publicId,
                Folder = "Images",
                File = new FileDescription()
                {
                    FileName = file.FileName,
                    Stream = file.OpenReadStream(),
                }
            };

            var uploadResult = await _cloudinary.UploadAsync(param);

            if (uploadResult.Error is null)
            {
                return new MediaOperationResult()
                {
                    PublicId = publicId,
                    Url = uploadResult.SecureUrl.ToString(),
                    result = true
                };
            }

            return new MediaOperationResult()
            {
                result = false,
                Errors = new List<string>() { "The Operation is not Accomplished" }
            };
        }

        public async Task<MediaOperationResult> UploadVideoAsync(IFormFile File)
        {
            var isValid = VideoValidationFilter(File);

            if (isValid)
            {
                var publicId = Guid.NewGuid().ToString();

                var param = new VideoUploadParams()
                {
                    PublicId = publicId,
                    Folder = "Videos",
                    File = new FileDescription()
                    {
                        FileName = File.FileName,
                        Stream = File.OpenReadStream(),
                    }
                };

                var results = await _cloudinary.UploadAsync(param);

                if (results.Error is null)
                    return new MediaOperationResult()
                    {
                        PublicId = publicId,
                        Url = results.SecureUrl.ToString(),
                        result = true
                    };
            }
            return new MediaOperationResult()
            {
                Errors = new List<string>() { "The Operation is not Accomplished" },
                result = false
            };
        }

        public async Task<MediaOperationResult> UploadAudioAsync(IFormFile file)
        {
            var isValid = AudioValidationFilter(file);

            if (isValid)
            {
                var publicId = Guid.NewGuid().ToString();

                var param = new RawUploadParams()
                {
                    PublicId = publicId,
                    Folder = "Audios",
                    File = new FileDescription()
                    {
                        FileName = file.FileName,
                        Stream = file.OpenReadStream(),
                    }
                };

                var results = await _cloudinary.UploadAsync(param);

                if (results.Error is null)
                    return new MediaOperationResult()
                    {
                        PublicId = publicId,
                        Url = results.SecureUrl.ToString(),
                        result = true
                    };
            }
            return new MediaOperationResult()
            {
                Errors = new List<string>()
                {
                    "The Operation is not Accomplished"
                },
                result = false
            };
        }

        public async Task<MediaOperationResult> RemoveFileAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                return new MediaOperationResult()
                {
                    result = false,
                    Errors = new List<string>() { "There is no public ID" }
                };


            var IsExist = await _context.cloudinaryUploads
                .FirstOrDefaultAsync(x => x.PublicId == publicId);

            if (IsExist is null)
                return new MediaOperationResult()
                {
                    result = false,
                    Errors = new List<string>() { "File not exist" }
                };

            var param = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Auto
            };

            var results = await _cloudinary.DestroyAsync(param);

            if (results.Error is null)
                return new MediaOperationResult
                {
                    result = true,
                    PublicId = publicId,
                };

            return new MediaOperationResult()
            {
                result = false,
                Errors = new List<string> { "The Operation is not Accomplished" }
            };
        }

        public async Task<MediaOperationResult> UpdateFileAsync(IFormFile file, string publicId, CloudinaryDotNet.Actions.ResourceType resourceType)
        {
            bool isValid = resourceType switch
            {
                ResourceType.Image => ImageValidationFilter(file),
                ResourceType.Video => VideoValidationFilter(file),
                _ => AudioValidationFilter(file),
            };

            if (!isValid)
                return new MediaOperationResult()
                {
                    result = false,
                    Errors = new List<string>() { "The Operation is not Accomplished" }
                };


            var cloudinaryResult = await _context.cloudinaryUploads.FirstOrDefaultAsync(x => x.PublicId == publicId);

            if (cloudinaryResult is null)
                return new MediaOperationResult()
                {
                    result = false,
                    Errors = new List<string>() { "The Operation is not Accomplished" }
                };


            using var stream = file.OpenReadStream();

            var uploadParams = resourceType switch
            {
                ResourceType.Image => new ImageUploadParams { PublicId = publicId, Folder = "Images", File = new FileDescription(file.FileName, stream) },
                ResourceType.Video => new VideoUploadParams { PublicId = publicId, Folder = "Videos", File = new FileDescription(file.FileName, stream) },
                _ => new RawUploadParams { PublicId = publicId, Folder = "Audios", File = new FileDescription(file.FileName, stream) }
            };


            var results = await _cloudinary.UploadAsync(uploadParams);

            if (results.Error == null)
                return new MediaOperationResult()
                {
                    result = true,
                    PublicId = publicId,
                    Url = results.SecureUrl.ToString(),
                };

            return new MediaOperationResult()
            {
                result = false,
                Errors = new List<string>() { "The Operation not Accomplished" }
            };
        }

        public async Task<MediaOperationResult> UploadImageToServerAsync(IFormFile file)
        {
            var isValidFile = ImageValidationFilter(file);

            if (!isValidFile)
                return InvalidFileResult();

            return await SaveFileToServerAsync(file, "Images");
        }

        public async Task<MediaOperationResult> UploadAudioToServerAsync(IFormFile file)
        {
            var isValidFile = AudioValidationFilter(file);

            if (!isValidFile)
                return InvalidFileResult();

            return await SaveFileToServerAsync(file, "Audios");
        }

        public async Task<MediaOperationResult> UploadVideoToServerAsync(IFormFile file)
        {
            var isValidFile = VideoValidationFilter(file);

            if (!isValidFile)
                return InvalidFileResult();

            return await SaveFileToServerAsync(file, "Videos");
        }

        public async Task<MediaOperationResult> SaveFileToServerAsync(IFormFile file, string folder)
        {

            try
            {
                var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(file.FileName)}";
                var directory = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", folder);
                var FullPath = Path.Combine(directory, fileName);

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                using (var stream = new FileStream(FullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return new MediaOperationResult
                {
                    result = true,
                    Url = $"/Uploads/{folder}/{fileName}",
                    PublicId = fileName
                };
            }
            catch (Exception ex)
            {
                return new MediaOperationResult()
                {
                    result = false,
                    Errors = new List<string>() { "Faild to save the file" }
                };
            }
        }

        private MediaOperationResult InvalidFileResult()
        {
            return new MediaOperationResult()
            {
                result = false,
                Errors = new List<string>() { "Invalid file" }
            };
        }
    }
}