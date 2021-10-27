using SuperfastBlur;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace ColorTool
{
    public abstract class ImageFilter
    {
        public abstract void Process(ref LockBitmap image);
    }

    public class EmptyFilter : ImageFilter
    {
        public override void Process(ref LockBitmap image) { }
    }

    public class GrayscaleFilter : ImageFilter
    {
        public override void Process(ref LockBitmap image)
        {
            image.LockBits();
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    var pixel = image.GetPixelBytes(x, y);
                    var grayValue = (int)Math.Round(((double)pixel[2] + pixel[1] + pixel[0]) / 3d);
                    image.SetPixel(x, y, Color.FromArgb(255, grayValue, grayValue, grayValue));
                }
            }
        }
    }

    public class PerceptualGrayscaleFilter : ImageFilter
    {
        public override void Process(ref LockBitmap image) 
        {
            image.LockBits();
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    var pixel = image.GetPixelBytes(x, y);
                    var grayValue = (int)Math.Round(pixel[2] * 0.299 + pixel[1] * 0.587 + pixel[0] * 0.114);
                    image.SetPixel(x, y, Color.FromArgb(255, grayValue, grayValue, grayValue));
                }
            }
        }
    }

    public class BlurFilter : ImageFilter
    {
        private static Color3[,] buffer = new Color3[5000, 5000];

        private readonly ParallelOptions _pOptions = new ParallelOptions { MaxDegreeOfParallelism = 16 };

        public int radial = 10;

        public BlurFilter(int blurSize)
        {
            this.radial = blurSize;
        }

        //public override void Process(LockBitmap image)
        //{
        //    // look at every pixel in the blur rectangle
        //    for (int xx = 0; xx < image.Width; xx++)
        //    {
        //        for (int yy = 0; yy < image.Height; yy++)
        //        {
        //            double avgR = 0, avgG = 0, avgB = 0;
        //            int blurPixelCount = 0;
        //            // average the color of the red, green and blue for each pixel in the
        //            // blur size while making sure you don't go outside the image bounds
        //            for (int x = xx; (x < xx + radial && x < image.Width); x++)
        //            {
        //                for (int y = yy; (y < yy + radial && y < image.Height); y++)
        //                {
        //                    var pixel = image.GetPixelBytes(x, y);
        //                    avgB += pixel[0];
        //                    avgG += pixel[1];
        //                    avgR += pixel[2];
        //                    blurPixelCount++;
        //                }
        //            }

        //            buffer[xx, yy] = new Color3(avgR / blurPixelCount, avgG / blurPixelCount, avgB / blurPixelCount);
        //        }
        //    }

        //    for (int x = 0; x < image.Width; x++)
        //    {
        //        for (int y = 0; y < image.Height; y++)
        //        {
        //            image.SetPixel(x, y, buffer[x, y]);
        //        }
        //    }
        //}

        public override void Process(ref LockBitmap image)
        {
            image.UnlockBits();
            var blur = new GaussianBlur(image.source);
            image = new LockBitmap(blur.Process(radial));
        }
    }


    public class ScaleDownFilter : ImageFilter
    {
        private static Color3[,] buffer = new Color3[5000, 5000];

        public int scaleDownSize = 2;

        public ScaleDownFilter(int scaleDownSize)
        {
            this.scaleDownSize = scaleDownSize;
        }

        public override void Process(ref LockBitmap image)
        {
            image.LockBits();
            var sWidth = image.Width / scaleDownSize;
            var sHeight = image.Height / scaleDownSize;
            
            // look at every pixel in the blur rectangle
            for (int xx = 0; xx < sWidth; xx++)
            {
                for (int yy = 0; yy < sHeight; yy++)
                {
                    double avgR = 0, avgG = 0, avgB = 0;
                    int blurPixelCount = 0;
                    // average the color of the red, green and blue for each pixel in the
                    // blur size while making sure you don't go outside the image bounds
                    var xp = xx * scaleDownSize;
                    var yp = yy * scaleDownSize;
                    for (int x = xp; (x < xp + scaleDownSize && x < image.Width); x++)
                    {
                        for (int y = yp; (y < yp + scaleDownSize && y < image.Height); y++)
                        {
                            var pixel = image.GetPixelBytes(x, y);
                            avgB += pixel[0];
                            avgG += pixel[1];
                            avgR += pixel[2];
                            blurPixelCount++;
                        }
                    }

                    buffer[xx, yy] = new Color3(avgR / blurPixelCount, avgG / blurPixelCount, avgB / blurPixelCount);
                }
            }

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    image.SetPixel(x, y, buffer[x % sWidth, y % sHeight]);
                }
            }
        }
    }

}
