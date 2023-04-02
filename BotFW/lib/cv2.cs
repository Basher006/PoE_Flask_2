using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BotFW.lib
{
    public class cv2
    {
        public static Size defloatKsize_for_blur = new Size(3, 3);
        public static Size defloatKernel_for_erode = new Size(2, 2);

        public struct imgFindRes
        {
            public Point[] result;
            public double maxRes;
        }

        public static Mat AppplyBlur(Mat img, Size ksize, double sigmaX = 1.0, double sigmaY = 1.0)
        {
            Mat outImage = img.Clone();
            CvInvoke.GaussianBlur(img, outImage, ksize, sigmaX, sigmaY);
            return outImage;
        }
        public static Mat ApplyErode(Mat img, Size Kernel, int iterations = 1)
        {
            var element = CvInvoke.GetStructuringElement(ElementShape.Cross, Kernel, new Point(-1, -1));
            Mat outImage = img.Clone();
            CvInvoke.Erode(outImage, outImage, element, new Point(-1, -1), iterations, BorderType.Constant, default(MCvScalar));
            return outImage;
        }

        public static Mat ConvertToGray(Mat img)
        {

            Mat outImage = new Mat(img.Height, img.Width, DepthType.Cv8U, 1);
            //Mat outImage = new Mat();
            if (img.NumberOfChannels == 3)
                CvInvoke.CvtColor(img, outImage, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
            else if (img.NumberOfChannels == 4)
                CvInvoke.CvtColor(img, outImage, Emgu.CV.CvEnum.ColorConversion.Rgba2Gray);

            return outImage;
        }
        public static void ConvertToGray(Mat img, Mat outImage)
        {
            if (img.NumberOfChannels == 3)
                CvInvoke.CvtColor(img, outImage, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
            else if (img.NumberOfChannels == 4)
                CvInvoke.CvtColor(img, outImage, Emgu.CV.CvEnum.ColorConversion.Rgba2Gray);
        }
        public static Mat ConvertToColor(Mat img)
        {
            Mat outImage = img.Clone();
            CvInvoke.CvtColor(outImage, outImage, Emgu.CV.CvEnum.ColorConversion.Gray2Bgr);
            return outImage;
        }
        public static Mat ConvertToHSV(Mat img)
        {
            Mat outImage = img.Clone();
            CvInvoke.CvtColor(outImage, outImage, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);
            return outImage;
        }
        public static Bitmap Convert_Mat_To_Bitmap(Mat img)
        {
            Image<Bgr, byte> cvImage = img.ToImage<Bgr, byte>();
            return cvImage.AsBitmap();
        }
        public static Mat Convert_Bitmap_To_Mat(Bitmap img)
        {
            return img.ToMat();
        }
        public static Mat Convert_to_conturs(Mat img, int ksize = 3, double skale = 1.0, double delta = 0.0)
        {
            DepthType depthType_16 = DepthType.Cv16S; // DepthType.Cv16S;
            DepthType depthType = DepthType.Cv8U;
            BorderType borderType = BorderType.Default; // Wrap-, NegativeOne-, Constant+, Isolated+, Reflect+, 

            int l = 35;
            int u = 256;

            int blureKsize = 5;

            int iterCount = 3;

            Mat blur_img = AppplyBlur(img, new Size { Height = blureKsize, Width = blureKsize });

            for (int i = 0; i < iterCount; i++)
            {
                blur_img = AppplyBlur(blur_img, new Size { Height = blureKsize, Width = blureKsize });
                CvInvoke.InRange(blur_img, (ScalarArray)new MCvScalar { V0 = l }, (ScalarArray)new MCvScalar { V0 = u }, blur_img);
            }
            

            blur_img.Save("blure_test_y.png");

            Mat grad_x = img.Clone();
            Mat grad_y = img.Clone();
            Mat abs_grad_x = img.Clone();
            Mat abs_grad_y = img.Clone();
            Mat out_img = img.Clone();

            CvInvoke.Sobel(blur_img, grad_x, depthType_16, 1, 0, ksize, skale, delta, borderType);
            CvInvoke.Sobel(blur_img, grad_y, depthType_16, 0, 1, ksize, skale, delta, borderType);

            CvInvoke.ConvertScaleAbs(grad_x, abs_grad_x, 1, 0);
            CvInvoke.ConvertScaleAbs(grad_y, abs_grad_y, 1, 0);

            CvInvoke.AddWeighted(grad_x, 1, grad_y, 1, 0, out_img, depthType);

            //Mat out_img_cv8u = new Mat
            //out_img.Save("cv16s.png");
            //Bitmap temp = out_img.ToBitmap();
            //temp.Save("temp.png");
            ////out_img = img.Clone();
            //out_img = temp.ToMat();
            ////out_img.ConvertTo(out_img, DepthType.Cv8U);
            ////Mat out_img_2 = new Mat(img.Width, img.Height, Emgu.CV.CvEnum.DepthType.Cv8U, 1);
            //Mat out_img_2 = BotFW.ConvertToGray(out_img);
            return out_img;
        }
        public static imgFindRes img_find_max_s(Mat screen, Mat templ, double tr, int CountTR = 200, Emgu.CV.CvEnum.TemplateMatchingType method = Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed)
        {
            Mat result = getTempl(screen, templ, method);
            double[] minV, maxV;
            Point[] minL, maxL;
            result.MinMax(out minV, out maxV, out minL, out maxL);

            imgFindRes outRes = new imgFindRes();

            Point[] output = new Point[1]
                {
                    new Point{X = -1, Y= -1},
                };
            outRes.maxRes = maxV.Max();

            if (maxV[0] > tr)
            {
                if (CountResult(result) > CountTR)
                {
                    Console.WriteLine("Warning! Find image too much results!");
                    result.Dispose();
                    outRes.result = output;
                    return outRes;
                }
                output[0].X = maxL[0].X;
                output[0].Y = maxL[0].Y;

                result.Dispose();
                outRes.result = output;
                return outRes;
            }
            result.Dispose();
            outRes.result = output;
            return outRes;

        }
        public static Point[] img_find_max(Mat screen, Mat templ, double tr, bool prtMax = false, int CountTR = 200, Emgu.CV.CvEnum.TemplateMatchingType method = Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed)
        {
            Mat result = getTempl(screen, templ, method);
            double[] minV, maxV;
            Point[] minL, maxL;
            result.MinMax(out minV, out maxV, out minL, out maxL);
            
            Point[] output = new Point[1]
                {
                    new Point{X = -1, Y= -1},
                };
            if (prtMax)
            {
                Console.WriteLine(maxV.Max());
            }
            
            if (maxV[0] > tr)
            {
                if (CountResult(result) > CountTR)
                {
                    Console.WriteLine("Warning! Find image too much results!");
                    result.Dispose();
                    return output;
                }
                output[0].X = maxL[0].X;
                output[0].Y = maxL[0].Y;

                result.Dispose();
                return output;
            }
            result.Dispose();
            return output;

        }
        public static imgFindRes img_find_all_s(Mat screen, Mat templ, float tr, Emgu.CV.CvEnum.TemplateMatchingType method = Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed)
        {
            Mat result = getTempl(screen, templ, method);
            double[] minV, maxV;
            Point[] minL, maxL;
            result.MinMax(out minV, out maxV, out minL, out maxL);
            imgFindRes outRes = new imgFindRes();
            outRes.maxRes = maxV.Max();

            Point[] output = get_rectangles(result, tr, templ.Width, templ.Height);
            outRes.result = output;
            return outRes;
        }
        public static Point[] img_find_all(Mat screen, Mat templ, float tr, bool prtMax = false, Emgu.CV.CvEnum.TemplateMatchingType method = Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed)
        {
            Mat result = getTempl(screen, templ, method);
            double[] minV, maxV;
            Point[] minL, maxL;
            result.MinMax(out minV, out maxV, out minL, out maxL);
            if (prtMax)
            {
                Console.WriteLine(maxV.Max());
            }

            Point[] outres = get_rectangles(result, tr, templ.Width, templ.Height);
            return outres;
        }
        public static Mat ApplyContrastBrightness(Mat img, float contrast = 1.0f, int brightness = 0)
        {
            Mat outMat = img.Clone();
            brightness += (int)Math.Round(255 * (1 - contrast) / 2);
            CvInvoke.AddWeighted(img, contrast, img, 0, brightness, outMat);
            return outMat;
        }
        public static Mat ApplyMask(Mat img, MCvScalar lower, MCvScalar upper, bool convert_to_hsv = false)
        {
            Mat outimg = new Mat(img.Height, img.Width, DepthType.Cv8U, 1);
            if (convert_to_hsv)
            {
                img = ConvertToHSV(img);
            }

            CvInvoke.InRange(img, (ScalarArray)lower, (ScalarArray)upper, outimg);

            return outimg;
        }
        public static List<Rectangle> FindAreaConturs(Mat img, int areaTrMin, int areaTrMax, int periTrMin, int periTRMax,
            RetrType mode = RetrType.External, ChainApproxMethod method = ChainApproxMethod.ChainApproxNone)
        {
            Mat hier = new Mat();
            List<Rectangle> outRes = new List<Rectangle>();
            VectorOfVectorOfPoint countours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(img, countours, hier, mode, method);

            for (int i = 0; i < countours.Size; i++)
            {
                double area = CvInvoke.ContourArea(countours[i]);
                if (area >= areaTrMin && area <= areaTrMax)
                {
                    double peri = CvInvoke.ArcLength(countours[i], true);
                    if (peri >= periTrMin && peri <= periTRMax)
                    {
                        VectorOfPoint apprx = new VectorOfPoint();
                        CvInvoke.ApproxPolyDP(countours[i], apprx, 0.02 * peri, true);
                        Rectangle result = CvInvoke.BoundingRectangle(apprx);
                        outRes.Add(result);
                    }
                }
            }
            return outRes;
        }
        public static List<Rectangle> FindAreaConturs(Mat img, Mat draw_img, MCvScalar color, int areaTrMin, int areaTrMax, int periTrMin, int periTRMax,
            RetrType mode = RetrType.External, ChainApproxMethod method = ChainApproxMethod.ChainApproxNone)
        {
            Mat hier = new Mat();
            List<Rectangle> outRes = new List<Rectangle>();
            VectorOfVectorOfPoint countours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(img, countours, hier, mode, method);

            for (int i = 0; i < countours.Size; i++)
            {
                double area = CvInvoke.ContourArea(countours[i]);
                if (area >= areaTrMin && area <= areaTrMax)
                {
                    CvInvoke.DrawContours(draw_img, countours, -1, new MCvScalar(0, 255, 0), 1);
                    double peri = CvInvoke.ArcLength(countours[i], true);
                    if (peri >= periTrMin && peri <= periTRMax)
                    {
                        VectorOfPoint apprx = new VectorOfPoint();
                        CvInvoke.ApproxPolyDP(countours[i], apprx, 0.02 * peri, true);
                        Rectangle result = CvInvoke.BoundingRectangle(apprx);
                        outRes.Add(result);

                        CvInvoke.Rectangle(draw_img, result, new MCvScalar(255, 0, 255), 1);
                    }
                }
            }
            return outRes;
        }
        //###########################
        private static Mat getTempl(Mat screen, Mat templ, TemplateMatchingType method = TemplateMatchingType.CcorrNormed)
        {
            Size size = new Size(screen.Width - templ.Width + 1, screen.Height - templ.Height + 1);
            Mat outImage = new Mat(size, DepthType.Cv8U, 1);
            CvInvoke.MatchTemplate(screen, templ, outImage, method);
            return outImage;
        }
        private static int CountResult(Mat result, double tr=0.999)
        {
            //var loc = new List<Rectangle>();
            int counter = 0;

             float[,] data1 = (float[,])result.GetData();
            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    if (data1[y, x] >= tr)
                    {
                        //Console.WriteLine("value" + data1[y, x] + ", bool:" + (data1[y, x] >= tr));
                        //loc.Add(new Rectangle(x, y, w, h));
                        //loc.Add(new Rectangle(x, y, w, h));
                        counter++;
                    }
                }
            }
            return counter;
        }
        private static bool ChekResult(Mat result, double tr, int acceptTR = 300)
        {
            //var loc = new List<Rectangle>();
            int counter = 0;

            float[,] data1 = (float[,])result.GetData();
            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    if (data1[y, x] >= tr)
                    {
                        //Console.WriteLine("value" + data1[y, x] + ", bool:" + (data1[y, x] >= tr));
                        //loc.Add(new Rectangle(x, y, w, h));
                        //loc.Add(new Rectangle(x, y, w, h));
                        counter++;
                    }
                }
            }
            return (counter < acceptTR);
        }
        private static Point[] get_rectangles(Mat result, float tr, int w, int h)
        {
            var loc = new List<Rectangle>();

            Point[] output = new Point[1]
            {
                    new Point{X = -1, Y= -1},
            };

            float[,] data1 = (float[,])result.GetData();
            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    if (data1[y, x] >= tr)
                    {
                        //Console.WriteLine("value" + data1[y, x] + ", bool:" + (data1[y, x] >= tr));
                        loc.Add(new Rectangle(x, y, w, h));
                        loc.Add(new Rectangle(x, y, w, h));
                    }
                }
            }
            if (loc.Count > 500)
            {
                Console.WriteLine("ERROR! Img find finded more that 500 items");
                return output;
            }
            var rectGroup = new VectorOfRect(loc.ToArray());
            CvInvoke.GroupRectangles(rectGroup, 1, 0.5);
            var rectGroupArr = rectGroup.ToArray();
            output = new Point[rectGroupArr.Length];
            for (int i = 0; i < rectGroupArr.Length; i++)
            {
                //output.Append(new Point(rectGroupArr[i].X, rectGroupArr[i].Y));
                output[i].X = rectGroupArr[i].X;
                output[i].Y = rectGroupArr[i].Y;
            }
            if (output.Length > 0)
                return output;
            else
                return new Point[1]
                {
                    new Point{X = -1, Y= -1},
                };
        }
    }
}
