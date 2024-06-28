using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        private int _currentImageIndex = 0;

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

            _dataContext = new DataContext(_config, factory.CreateLogger<DataContext>());

            _logger = factory.CreateLogger<MainWindow>();
            _imageFiles = _dataContext.DiscoverImages();
            _currentImageIndex = 0;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetBackgroundByIndex();
            ResizeWindowToFitImage();

            _logger.LogInformation("Application loaded");
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
            var imageObj = _imageFiles.ElementAtOrDefault(_currentImageIndex);
            if (imageObj == null)
            {
                _logger.LogWarning("No image found at index {Index}", _currentImageIndex);
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

            lblStatus.Text = $"Image {_currentImageIndex + 1} of {_imageFiles.Count()}";
        }

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            _logger.LogInformation("User clicked Previous");
            _currentImageIndex-= _currentImageIndex == 0 ? 0 : 1;
            SetBackgroundByIndex();
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            _logger.LogInformation("User clicked Yes");
            _currentImageIndex += _currentImageIndex == _imageFiles.Count() - 1 ? 0 : 1;
            SetBackgroundByIndex();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            _logger.LogInformation("User clicked No");
            _currentImageIndex += _currentImageIndex == _imageFiles.Count() - 1 ? 0 : 1;
            SetBackgroundByIndex();
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            _logger.LogInformation("Exiting application");
            Application.Current.Shutdown();
        }
    }
}