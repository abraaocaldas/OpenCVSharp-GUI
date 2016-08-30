using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using OpenCvSharp.CPlusPlus;
using OpenCVSharp_GUI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _operationOrder.CollectionChanged += operationOrder_CollectionChanged;
        }

        private void operationOrder_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
           
        }

        private void enableCanny_Checked(object sender, RoutedEventArgs e)
        {
            showToolTipWindow();
        }

        private void enableCanny_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private async void showToolTipWindow()
        {
            await this.ShowChildWindowAsync(new ToolTipWindow() { IsModal = true, AllowMove = true }, LayoutRoot);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
