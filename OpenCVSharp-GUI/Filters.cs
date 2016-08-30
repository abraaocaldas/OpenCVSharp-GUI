using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVSharp_GUI
{
    public sealed class Filters
    {
        private static readonly Lazy<Filters> lazy = new Lazy<Filters>(() => new Filters());

        public static Filters Instance { get { return lazy.Value; } }


        private static double ThresholdValue;
        private static int AdaptiveVal1;
        private static int AdaptiveVal2;
        private static int ResizeValue;

        public static void ErodeImage(IplImage gray, ref IplImage eroded)
        {
            OpenCvSharp.Cv.Erode(gray, eroded);
        }

        public static void DilateImage(IplImage gray, ref IplImage dilated)
        {
            OpenCvSharp.Cv.Dilate(gray, dilated);
        }

        public static void HistogramEqualize(IplImage gray, ref IplImage equalized)
        {
            OpenCvSharp.Cv.EqualizeHist(gray, equalized);
        }

        public static void SetAdaptTreshold(IplImage gray, ref IplImage threshold)
        {
            gray.AdaptiveThreshold(threshold, ThresholdValue, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, AdaptiveVal1, AdaptiveVal2);
        }

        public static void EdgeEnhancement(IplImage gray, ref IplImage enhancedImage)
        {
            float[] data = { -1, -1, -1, -1, -1, -1, 2, 2, 2, -1,
                                     -1,2,8,2,-1, -1,2,2,2,-1, -1,-1,-1,-1,-1
                    };
            CvMat kernel = new CvMat(5, 5, MatrixType.U8C1, data);
            Cv.Normalize(kernel, kernel, 8, 0, NormType.L1);
            OpenCvSharp.Cv.Filter2D(gray, enhancedImage, kernel);
        }

        public static void CannyFilter(IplImage gray, ref IplImage canny, int value1, int value2)
        {
            Cv.Canny(gray, canny, value1, value2);
        }

        //private static void UpdateCanny()
        //{
        //    if (operationOrder.Count > 0)
        //    {
        //        List<String> temp = new List<string>();
        //        temp = operationOrder.Take(operationOrder.Count).ToList();
        //        operationOrder.Clear();
        //        operationOrder = new ObservableCollection<String>(temp.Take(temp.Count - 1).ToList());
        //        operationOrder.CollectionChanged += OperationOrder_CollectionChanged;
        //        operationOrder.Add(temp.Last());
        //    }
        //}

        public static void Denoiser(IplImage gray, ref IplImage denoised)
        {
            Mat nGray = new Mat(gray);
            Mat nOutput = new Mat();
            Cv2.FastNlMeansDenoising(nGray, nOutput);

            denoised = nOutput.ToIplImage().Clone();
        }

        public static void ScaleImage(IplImage gray, ref IplImage scalled)
        {
            double RValue = Double.Parse(ResizeValue.ToString());
            int width = (int)(gray.Width * (RValue / 100));
            int height = (int)(gray.Height * (RValue / 100));
            scalled = new IplImage(new CvSize(width, height), BitDepth.U8, 1);
            gray.Resize(scalled, Interpolation.Linear);
        }

        public static void InvertImage(IplImage gray, ref IplImage inverted)
        {
            Mat temp = new Mat(gray);
            Mat tempOut = new Mat();
            Cv2.BitwiseNot(temp, tempOut);
            tempOut.ToIplImage().Copy(inverted);
        }
    }
}
