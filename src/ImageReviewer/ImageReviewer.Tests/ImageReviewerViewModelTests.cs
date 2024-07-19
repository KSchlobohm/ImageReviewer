
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.Extensions;

namespace ImageReviewer.Tests
{
    public class ImageReviewerViewModelTests
    {
        [Fact]
        public void ImageReviewerViewModel_CanCountTotalDetections()
        {
            var logger = Substitute.For<ILogger<ImageReviewerViewModel>>();
            var reader = Substitute.For<IYoloDataReader>();
            reader.DiscoverYoloDetections().ReturnsForAnyArgs(
                // for this test we assume the yolo file contains 3 images and 0 object detections
                Result<IEnumerable<ImageData>>.Success(new List<ImageData>
                {
                    new ImageData("file1.jpg", new List<ObjectDetection>()),
                    new ImageData("file2.jpg", new List<ObjectDetection>()),
                    new ImageData("file3.jpg", new List<ObjectDetection>())
                }));

            var vm = new ImageReviewerViewModel(logger, reader);

            vm.LoadDefaults();

            Assert.Equal(3, vm.FileNames.Length);
        }
    }
}