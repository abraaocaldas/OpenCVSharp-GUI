using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;
using OpenCVSharp_GUI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenCVSharp_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        private ObservableCollection<String> _operationOrder = new ObservableCollection<String>();
        private Mat _originalMat;
        private Mat _convertedMat;
        private BitmapSource _originalImage;
        private BitmapSource _convertedImage;
        private int _cannyValue1;
        private int _cannyValue2;
        private int _thrsvalue;
        private int _adpt1;
        private int _adpt2;
        private int _resizeValue;

        #region Public Properties
        public BitmapSource OriginalImage
        {
            get
            {
                return _originalImage;
            }
            set
            {
                _originalImage = value;
                OnPropertyChanged("OriginalImage");
            }
        }

        public BitmapSource ConvertedImage
        {
            get
            {
                return _convertedImage;
            }
            set
            {
                _convertedImage = value;
                OnPropertyChanged("ConvertedImage");
            }
        }

        public int CannyValue1
        {
            get { return _cannyValue1; }
            set
            {
                _cannyValue1 = value;
                OnPropertyChanged("CannyValue1");
            }
        }

        public int CannyValue2
        {
            get { return _cannyValue2; }
            set
            {
                _cannyValue2 = value;
                OnPropertyChanged("CannyValue2");
            }
        }

        public int ResizeValue
        {
            get
            {
                return _resizeValue;
            }
            set
            {
                _resizeValue = value;
                OnPropertyChanged("ResizeValue");
            }
        }

        public int AdaptiveVal1
        {
            get
            {
                return _adpt1;
            }
            set
            {
                _adpt1 = value;
                OnPropertyChanged("AdaptiveVal1");
            }
        }

        public int AdaptiveVal2
        {
            get
            {
                return _adpt2;
            }
            set
            {
                _adpt2 = value;
                OnPropertyChanged("AdaptiveVal2");
            }
        }

        public int ThresholdValue
        {
            get
            {
                return _thrsvalue;
            }
            set
            {
                _thrsvalue = value;
                OnPropertyChanged("ThresholdValue");
            }
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _operationOrder.CollectionChanged += operationOrder_CollectionChanged;
        }

        #region Private Methods
        private void operationOrder_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _convertedMat = _originalMat.Clone();
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var item in _operationOrder)
                {
                    ExecuteTransformation(_convertedMat, item);
                }
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                if (_operationOrder.Count > 0)
                {
                    foreach (var item in _operationOrder)
                    {
                        ExecuteTransformation(_convertedMat, item);
                    }
                }
                else
                {
                    ConvertedImage = OriginalImage;
                }

            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            {
                ConvertedImage = OriginalImage;
            }
        }

        private void ExecuteTransformation(Mat image, String transformation)
        {
            IplImage gray = new IplImage(Cv.GetSize(((IplImage)image)), BitDepth.U8, 1);
            IplImage dest = new IplImage(Cv.GetSize(((IplImage)image)), BitDepth.U8, 1);

            if (((IplImage)image).NChannels > 1)
            {
                ((IplImage)image).CvtColor(gray, ColorConversion.BgrToGray);
            }
            else
            {
                gray = ((IplImage)image).Clone();
            }

            switch (transformation)
            {
                case "Histogram":
                    Filters.HistogramEqualize(gray, ref dest);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    break;
                case "Erode":
                    Filters.ErodeImage(gray, ref dest);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    break;
                case "EdgeEnhancement":
                    Filters.EdgeEnhancement(gray, ref dest);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    break;
                case "CannyFilter":
                    Filters.CannyFilter(gray, ref dest, CannyValue1, CannyValue2);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    break;
                case "Dilate":
                    Filters.DilateImage(gray, ref dest);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    break;
                case "Denoiser":
                    Filters.Denoiser(gray, ref dest);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    break;
                case "Resize":
                    Filters.ScaleImage(gray, ref dest, 10);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    break;
                case "BitWise Inverter":
                    Filters.InvertImage(gray, ref dest);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    break;
                case "AdaptiveThreshold":
                    Filters.SetAdaptTreshold(gray, ref dest, 10, 10, 10);
                    ConvertedImage = dest.ToBitmap().ToBitmapSource();
                    break;
                default:
                    break;
            }
            _convertedMat = new Mat(dest);
        }

        private void LoadImage(String Path)
        {
            _originalMat = new Bitmap(Path).ToMat();
            _convertedMat = _originalMat.Clone();
            OriginalImage = _originalMat.ToBitmapSource();
            ConvertedImage = _originalMat.ToBitmapSource();
            ResizeValue = 100;
        }
        #endregion

        #region Events
        private void enableCanny_Checked(object sender, RoutedEventArgs e)
        {
            _operationOrder.Add("CannyFilter");
        }

        private void enableCanny_Unchecked(object sender, RoutedEventArgs e)
        {
            _operationOrder.Remove("CannyFilter");
        }

        private void loadImage_Click(object sender, RoutedEventArgs e)
        {
            var dialogFile = new System.Windows.Forms.OpenFileDialog();
            var imageExtensions = string.Join(";", ImageCodecInfo.GetImageDecoders().Select(ici => ici.FilenameExtension));
            dialogFile.Filter = string.Format("Images|{0}|All Files|*.*", imageExtensions);
            var result = dialogFile.ShowDialog();
                        
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    String selectedFile = dialogFile.FileName;
                    if (!String.IsNullOrEmpty(selectedFile))
                    {
                        LoadImage(selectedFile);
                    }
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Window Events
        private async void showToolTipWindow()
        {
            await this.ShowChildWindowAsync(new ToolTipWindow() { IsModal = true, EnableDropShadow=true}, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void cannyQuestion_Click(object sender, RoutedEventArgs e)
        {
            showToolTipWindow();
        }
    }
}
