using Microsoft.Extensions.Logging;
using System.IO;

namespace ImageReviewer
{
    public class YoloDataReader : IYoloDataReader
    {
        private ILogger _logger;
        private YoloConfigManager _configManager;

        public YoloDataReader(ILogger<YoloDataReader> logger, YoloConfigManager configManager)
        {
            _logger = logger;
            _configManager = configManager;
        }

        public Result<IEnumerable<ImageData>> DiscoverYoloDetections()
        {
            if (_configManager is null || _configManager.Config is null)
            {
                _logger.LogError("YoloDataReader: Yolo config was not available");
                return Result<IEnumerable<ImageData>>.Failure("Yolo config was not available");
            }

            var pathToYaml = _configManager.YoloConfigFilePath;
            if (string.IsNullOrEmpty(pathToYaml))
            {
                _logger.LogError("YoloDataReader: Yolo config file path was not available");
                return Result<IEnumerable<ImageData>>.Failure("Yolo config file path was not available");
            }

            var trainFolder = Path.Combine(Path.GetDirectoryName(pathToYaml)!, _configManager.Config.TrainFolder);
            var validationFolder = Path.Combine(Path.GetDirectoryName(pathToYaml)!, _configManager.Config.ValidationFolder);

            var folderPaths = new List<string> { trainFolder, validationFolder };

            var jpgFiles = FindAllFiles(folderPaths, "*.jpg");
            var pngFiles = FindAllFiles(folderPaths, "*.png");

            var imageFiles = jpgFiles.Concat(pngFiles);
            _logger.LogInformation($"Found {imageFiles.Count()} total images");

            return Result<IEnumerable<ImageData>>.Success(imageFiles);
        }

        public string ConvertClassIdToLabel(int classId)
        {
            if (_configManager?.Config?.ClassNames is not null
                && 0 <= classId
                && classId < _configManager.Config.ClassNames.Length)
            {
                return _configManager.Config.ClassNames[classId];
            }

            return classId.ToString();
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
                    var detections = FindDetectionsForFile(file);
                    allFiles.Add(new ImageData(file, detections));
                }
            }

            return allFiles;
        }

        private List<ObjectDetection> FindDetectionsForFile(string fileName)
        {
            //Use ImageSharp to load the image from fileName
            var image = SixLabors.ImageSharp.Image.Load(fileName);

            // Assumes that the object detection data is stored in a file with the same name as the image
            // because that is how the Yolo training data is formatted

            var detectionFile = Path.Combine(Path.GetDirectoryName(fileName)!,"..", "labels", Path.GetFileName(fileName));
            detectionFile = Path.ChangeExtension(detectionFile, ".txt");
            if (!File.Exists(detectionFile))
            {
                _logger.LogWarning("Detection file {DetectionFile} does not exist", detectionFile);
                return new List<ObjectDetection>();
            }

            var detections = new List<ObjectDetection>();
            foreach(var line in File.ReadAllLines(detectionFile))
            {
                var detection = ParseLineAsObjectDetection(line, image.Height, image.Width);
                if (detection is not null)
                {
                    detections.Add(detection);
                }
            }
            return detections;
        }

        private ObjectDetection? ParseLineAsObjectDetection(string line, double imageHeight, double imageWidth)
        {
            var parts = line.Split(' ');
            if (parts.Length == 5)
            {
                int classId = int.Parse(parts[0]);
                int height = (int)(double.Parse(parts[3]) * imageHeight);
                int width = (int)(double.Parse(parts[4]) * imageWidth);
                int x = (int)(double.Parse(parts[1]) - width/2);
                int y = (int)(double.Parse(parts[2]) - height / 2);

                return new ObjectDetection
                {
                    ClassId = classId,
                    X = x,
                    Y = y,
                    Height = height,
                    Width = width,
                    IsValid = true //starts with the assumption that the detection is valid
                };
            }

            return null;
        }
    }
}
