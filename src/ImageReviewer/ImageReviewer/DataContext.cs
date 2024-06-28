using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;

namespace ImageReviewer
{
    public record BoundingBoxes(int X, int Y, int Width, int Height);
    public record ImageData(string Path, List<BoundingBoxes> Boxes);

    public class DataContext
    {
        private IConfiguration _config;
        private ILogger<DataContext> _logger;

        public DataContext(IConfiguration config, ILogger<DataContext> logger)
        {
            _config = config;
            _logger = logger;
        }

        public IEnumerable<ImageData> DiscoverImages()
        {
            var trainFolder = _config.GetValue<string>("DataFolder:train");
            var validationFolder = _config.GetValue<string>("DataFolder:val");
            if (trainFolder == null)
            {
                _logger.LogError("DataFolder:train is missing from the configuration");
                return Enumerable.Empty<ImageData>();
            }
            if (validationFolder == null)
            {
                _logger.LogError("DataFolder:val is missing from the configuration");
                return Enumerable.Empty<ImageData>();
            }

            var folderPaths = new List<string>
            {
                Path.Combine(trainFolder, "images"),
                Path.Combine(validationFolder, "images"),
            };

            var jpgFiles = FindAllFiles(folderPaths, "*.jpg");
            var pngFiles = FindAllFiles(folderPaths, "*.png");

            var imageFiles = jpgFiles.Concat(pngFiles);
            _logger.LogInformation($"Found {imageFiles.Count()} total images");

            return imageFiles;
        }

        private IEnumerable<ImageData> FindAllFiles(IEnumerable<string> folderPaths, string searchPattern)
        {
            var allFiles = new List<ImageData>();

            foreach (var folderPath in folderPaths)
            {
                if (string.IsNullOrEmpty(folderPath))
                {
                    _logger.LogError("Folder path is missing or empty");
                    continue;
                }

                _logger.LogInformation($"Looking for files in {folderPath}");

                if (!Directory.Exists(folderPath))
                {
                    _logger.LogWarning("Folder {FolderPath} does not exist", folderPath);
                    continue;
                }
                var files = Directory.GetFiles(folderPath, searchPattern);
                _logger.LogInformation($"Found {files.Length} files in {folderPath}");

                foreach (var file in files)
                {
                    allFiles.Add(new ImageData(file, new List<BoundingBoxes>()));
                }
            }

            return allFiles;
        }
    }
}
