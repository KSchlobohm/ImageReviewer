using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using YamlDotNet.Serialization;

namespace ImageReviewer
{
    public class YoloConfigManager
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public YoloConfigManager(IConfiguration config, ILogger<YoloConfigManager> logger)
        {
            _config = config;
            _logger = logger;
        }

        public string? YoloConfigFilePath { get; private set; }
        public YoloConfigFile? Config { get; private set; }

        private void TryLoadDefault()
        {
            var configFileName = string.Empty; // _config.GetSection("YoloYamlFile").Get<string>();
            var result = LoadFromFile(configFileName ?? string.Empty);
            if (result.IsSuccess && result.Value is not null)
            {
                Config = result.Value;
            }
        }

        public Result<YoloConfigFile> LoadFromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                _logger.LogError("Cannot load configuration from a file with an empty name");
                return Result<YoloConfigFile>.Failure("appsettings.json:YoloYamlFile");
            }
            if (!File.Exists(fileName))
            {
                _logger.LogError("Yolo config file not found: {FileName}", fileName);
                return Result<YoloConfigFile>.Failure("Yolo config file not found: " + fileName);
            }

            // deserialize from yaml into YoloData.cs
            var yaml = new DeserializerBuilder().Build();
            var yoloData = yaml.Deserialize<YoloConfigFile>(File.ReadAllText(fileName));

            Config = yoloData;
            YoloConfigFilePath = Path.GetFullPath(fileName);

            // sharing the result so that the caller can see success/failure
            return Result<YoloConfigFile>.Success(yoloData);
        }
    }
}
