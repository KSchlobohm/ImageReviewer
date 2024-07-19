using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageReviewer.Tests
{
    public class YoloConfigManagerTests
    {
        [Fact]
        public void YoloConfigManager_CanReadFromFile()
        {
            var configuration = Substitute.For<IConfiguration>();
            var logger = Substitute.For<ILogger<YoloConfigManager>>();
            var manager = new YoloConfigManager(configuration, logger);

            var config = manager.LoadFromFile("yolo-testfile.yaml");

            Assert.NotNull(config);
            Assert.True(config.IsSuccess);
            Assert.NotNull(config.Value);
            Assert.Equal(8, config.Value.ClassNames.Count());
        }
    }
}
