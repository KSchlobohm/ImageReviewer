using Microsoft.Extensions.Logging;

namespace ImageReviewer
{
    public class ImageReviewerViewModel
    {
        private ILogger _logger;
        private IYoloDataReader _reader;

        public ImageReviewerViewModel(ILogger<ImageReviewerViewModel> logger,
            IYoloDataReader reader)
        {
            _logger = logger;
            _reader = reader;
        }

        /// <summary>
        /// For each file name, there is a collection of detections.
        /// </summary>
        public IEnumerable<ImageData> YoloImageData { get; set; } = new List<ImageData>();

        /// <summary>
        /// A collection of all file names that are being reviewed.
        /// These files include the full path and file name.
        /// </summary>
        public string[] FileNames => YoloImageData.Select(x => x.Path).ToArray();

        public void LoadDefaults()
        {
            var result = _reader.DiscoverYoloDetections();
            if (result.IsSuccess
                && result.Value is not null)
            {
                YoloImageData = result.Value;
            }
            else
            {
                _logger.LogWarning("No data was available by default. User must load a file");
            }
        }
    }
}
