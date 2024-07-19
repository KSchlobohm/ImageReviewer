using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ImageReviewer;

namespace ImageReviewerApp
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IConfiguration _config;
        private ILogger _logger;

        private ImageReviewerViewModel _vm;

        public MainWindow(IConfiguration config,
            ILogger<MainWindow> logger,
            ImageReviewerViewModel vm)
        {
            vm = _vm;
            InitializeComponent();
        }

        #region Event handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //// Load the default data based on appsettings.json
            //// if not successful, show an error to the user so they can load a file
            //var defaultContextResult = _dataContext.DiscoverImages();

            //if (defaultContextResult.IsSuccess
            //    && defaultContextResult.Value is not null)
            //{
            //    _imageFiles = defaultContextResult.Value;
            //    _currentDetectionIndex = 0;
            //    SetBackgroundByIndex();
            //    ResizeWindowToFitImage();
            //}
            //else
            //{
            //    _logger.LogWarning("No data was available by default. User must load a file");
            //    ShowErrorToUser("No data was available by default. Please load a file.");
            //}
        }

        private void LoadFromFile(string yamlFile)
        {
            //var result = _configManager.LoadFromFile(yamlFile);
            //if (result.IsSuccess)
            //{
            //    var dataResult = _dataContext.DiscoverImages();

            //    if (dataResult.IsSuccess
            //        && dataResult.Value is not null)
            //    {
            //        _imageFiles = dataResult.Value;
            //        _currentDetectionIndex = 0;
            //        SetBackgroundByIndex();
            //        ResizeWindowToFitImage();
            //    }
            //    else
            //    {
            //        ShowErrorToUser(dataResult.Error);
            //    }
            //}
            //else
            //{
            //    ShowErrorToUser(result.Error);
            //}
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            //ClearErrorShownToUser();
            //try
            //{
            //    // use a file dialog to select the yaml file
            //    var dialog = new OpenFileDialog
            //    {
            //        Filter = "Yaml files (*.yaml)|*.yaml|All files (*.*)|*.*"
            //    };
            //    if (dialog.ShowDialog() == true && File.Exists(dialog.FileName))
            //    {
            //        LoadFromFile(dialog.FileName);
            //    }
            //    else
            //    {
            //        throw new FileNotFoundException("Unable to load the yaml file");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error loading Yolo data");
            //    if (Debugger.IsAttached)
            //    {
            //        ShowErrorToUser($"Error loading Yolo data: {ex.Message}");
            //    }
            //    else
            //    {

            //        ShowErrorToUser($"Error loading Yolo data");
            //    }
            //}
        }

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            //ClearErrorShownToUser();
            //_logger.LogInformation("User clicked Previous");
            //_currentDetectionIndex -= _currentDetectionIndex == 0 ? 0 : 1;
            //SetBackgroundByIndex();
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            //ClearErrorShownToUser();
            //_logger.LogInformation("User clicked Yes");
            //_currentDetectionIndex += _currentDetectionIndex == _imageFiles.Sum(image => image.Detections.Count) - 1 ? 0 : 1;
            //SetBackgroundByIndex();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            //ClearErrorShownToUser();
            //_logger.LogInformation("User clicked No");
            //_currentDetectionIndex += _currentDetectionIndex == _imageFiles.Sum(image => image.Detections.Count) - 1 ? 0 : 1;
            //SetBackgroundByIndex();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            //ClearErrorShownToUser();
            //_logger.LogInformation("User clicked Next");
            //_currentDetectionIndex += _currentDetectionIndex == 0 ? 0 : 1;
            //SetBackgroundByIndex();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //ClearErrorShownToUser();
            //_logger.LogInformation("Saving results");
            //ShowErrorToUser("Not yet implemented");
        }
        #endregion

        private int GetCurrentImageIndex()
        {
            //var totalDetections = 0;
            //for (int i = 0; i < _imageFiles.Count(); i++)
            //{
            //    totalDetections += _imageFiles.ElementAt(i).Detections.Count;
            //    if (totalDetections > _currentDetectionIndex)
            //    {
            //        return i;
            //    }
            //}

            return 0;
        }

        private void ResizeWindowToFitImage()
        {
            //// set the window size to the size of the image
            //var image = canvas.Children.OfType<Image>().FirstOrDefault();
            //if (image == null)
            //{
            //    _logger.LogWarning("No image found to resize window");
            //    return;
            //}

            //this.Width = image.Source.Width;
            //this.Height = image.Source.Height + 100;
        }

        private void SetBackgroundByIndex()
        {
            //canvas.Children.Clear();
            //// use the _currentImageIndex to load the image from the _imageFiles
            //// and set it as the background of the canvas
            //var imageObj = _imageFiles.ElementAtOrDefault(GetCurrentImageIndex());
            //if (imageObj == null)
            //{
            //    _logger.LogWarning("No image found at index {Index}", GetCurrentImageIndex());
            //    return;
            //}

            //_logger.LogInformation("Setting background to {ImagePath}", imageObj.Path);
            //var bitmap = new BitmapImage(new Uri(imageObj.Path));

            //// Resize the image to 50% size
            //var resizedBitmap = new TransformedBitmap(bitmap, new ScaleTransform(0.5, 0.5));

            //var image = new Image
            //{
            //    Source = resizedBitmap
            //};
            //canvas.Children.Add(image);

            //var imageCountData = $"Image {GetCurrentImageIndex() + 1} of {_imageFiles.Count()}.";
            //var detectionCountData = $"Detection {_currentDetectionIndex + 1} of {_imageFiles.Sum(img => img.Detections.Count)}";
            //var question = $"Is this an instance of {GetClassIdText()}?";

            //lblStatus.Text = $"{imageCountData} {detectionCountData} {question}";
        }

        private string GetClassIdText()
        {
            //var classId = _imageFiles.SelectMany(img => img.Detections).ElementAt(_currentDetectionIndex).ClassId;
            //return _dataContext.ConvertClassIdToLabel(classId);
            return string.Empty;
        }

        private void ClearTextShownToUser()
        {
            lblStatus.Text = string.Empty;
            lblStatus.Background = new SolidColorBrush(Colors.Transparent);
            lblStatus.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void ShowWarningToUser(string message)
        {
            lblStatus.Text = message;
            lblStatus.Background = new SolidColorBrush(Colors.Red);
            lblStatus.Foreground = new SolidColorBrush(Colors.White);
        }

        private void ShowErrorToUser(string message)
        {
            lblStatus.Text = message;
            lblStatus.Background = new SolidColorBrush(Colors.Red);
            lblStatus.Foreground = new SolidColorBrush(Colors.White);
        }
    }
}