using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using YamlDotNet.Serialization;

namespace ImageReviewer
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IConfiguration _config;
        private ILogger<MainWindow> _logger;
        private DataContext _dataContext;
        private IEnumerable<ImageData> _imageFiles;
        private int _currentDetectionIndex = 0;

        public MainWindow()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<MainWindow>()
                .Build();

            using ILoggerFactory factory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            _logger = factory.CreateLogger<MainWindow>();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            using ILoggerFactory factory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });
            // deserialize from yaml into YoloData.cs
            var yamlFile = _config["YoloYaml"]!;
            var yaml = new DeserializerBuilder().Build();
            var yoloData = yaml.Deserialize<YoloData>(File.ReadAllText(yamlFile));
            yoloData.YoloConfigFilePath = yamlFile;

            _dataContext = new DataContext(factory.CreateLogger<DataContext>(), yoloData);
            _imageFiles = _dataContext.DiscoverImages();

            SetBackgroundByIndex();
            ResizeWindowToFitImage();

            _logger.LogInformation("Application loaded");
        }

        private int GetCurrentImageIndex()
        {
            var totalDetections = 0;
            for (int i = 0; i < _imageFiles.Count(); i++)
            {
                totalDetections += _imageFiles.ElementAt(i).Detections.Count;
                if (totalDetections > _currentDetectionIndex)
                {
                    return i;
                }
            }

            return 0;
        }

        private void ResizeWindowToFitImage()
        {
            // set the window size to the size of the image
            var image = canvas.Children.OfType<Image>().FirstOrDefault();
            if (image == null)
            {
                _logger.LogWarning("No image found to resize window");
                return;
            }

            this.Width = image.Source.Width;
            this.Height = image.Source.Height + 100;
        }

        private void SetBackgroundByIndex()
        {
            canvas.Children.Clear();
            // use the _currentImageIndex to load the image from the _imageFiles
            // and set it as the background of the canvas
            var imageObj = _imageFiles.ElementAtOrDefault(GetCurrentImageIndex());
            if (imageObj == null)
            {
                _logger.LogWarning("No image found at index {Index}", GetCurrentImageIndex());
                return;
            }

            _logger.LogInformation("Setting background to {ImagePath}", imageObj.Path);
            var bitmap = new BitmapImage(new Uri(imageObj.Path));

            // Resize the image to 50% size
            var resizedBitmap = new TransformedBitmap(bitmap, new ScaleTransform(0.5, 0.5));

            var image = new Image
            {
                Source = resizedBitmap
            };
            canvas.Children.Add(image);

            var imageCountData = $"Image {GetCurrentImageIndex() + 1} of {_imageFiles.Count()}.";
            var detectionCountData = $"Detection {_currentDetectionIndex + 1} of {_imageFiles.Sum(img => img.Detections.Count)}";
            var question = $"Is this an instance of {GetClassIdText()}?";

            lblStatus.Text = $"{imageCountData} {detectionCountData} {question}";
        }

        private string GetClassIdText()
        {
            var classId = _imageFiles.SelectMany(img => img.Detections).ElementAt(_currentDetectionIndex).ClassId;
            return _dataContext.ConvertClassIdToLabel(classId);
        }

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            _logger.LogInformation("User clicked Previous");
            _currentDetectionIndex -= _currentDetectionIndex == 0 ? 0 : 1;
            SetBackgroundByIndex();
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            _logger.LogInformation("User clicked Yes");
            _currentDetectionIndex += _currentDetectionIndex == _imageFiles.Sum(image => image.Detections.Count) - 1 ? 0 : 1;
            SetBackgroundByIndex();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            _logger.LogInformation("User clicked No");
            _currentDetectionIndex += _currentDetectionIndex == _imageFiles.Sum(image => image.Detections.Count) - 1 ? 0 : 1;
            SetBackgroundByIndex();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            _logger.LogInformation("User clicked Next");
            _currentDetectionIndex += _currentDetectionIndex == 0 ? 0 : 1;
            SetBackgroundByIndex();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _logger.LogInformation("Saving results");
        }
    }
}