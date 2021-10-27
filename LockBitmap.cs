using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ColorTool
{
    public class LockBitmap
    {
        public bool IsLocked { get; private set; }
        public Bitmap source = null;
        IntPtr Iptr = IntPtr.Zero;
        BitmapData bitmapData = null;

        public byte[] Pixels { get; set; }
        public int Depth { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        private int colorCount;


        public LockBitmap(Bitmap source)
        {
            this.source = source;
        }

        /// <summary>
        /// Lock bitmap data
        /// </summary>
        public void LockBits()
        {
            if (IsLocked)
                return;
            try
            {
                // Get width and height of bitmap
                Width = source.Width;
                Height = source.Height;

                // get total locked pixels count
                int PixelCount = Width * Height;

                // Create rectangle to lock
                Rectangle rect = new Rectangle(0, 0, Width, Height);

                // get source bitmap pixel format size
                Depth = System.Drawing.Bitmap.GetPixelFormatSize(source.PixelFormat);
                colorCount = Depth / 8;

                // Check if bpp (Bits Per Pixel) is 8, 24, or 32
                if (Depth != 8 && Depth != 24 && Depth != 32)
                {
                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                }

                // Lock bitmap and return bitmap data
                bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
                                             source.PixelFormat);

                // create byte array to copy pixel values
                int step = Depth / 8;
                if (Pixels == null || Pixels.Length != PixelCount * step)
                    Pixels = new byte[PixelCount * step];

                Iptr = bitmapData.Scan0;

                // Copy data from pointer to array
                Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
                IsLocked = true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Unlock bitmap data
        /// </summary>
        public void UnlockBits()
        {
            if (!IsLocked)
                return;
            try
            {
                // Copy data from byte array to pointer
                Marshal.Copy(Pixels, 0, Iptr, Pixels.Length);

                // Unlock bitmap data
                source.UnlockBits(bitmapData);
                IsLocked = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color GetPixel(int x, int y)
        {
            // Get start index of the specified pixel
            int i = ((y * Width) + x) * colorCount;

            //if (i > Pixels.Length - cCount)
            //    throw new IndexOutOfRangeException();

            switch (Depth) // For 32 bpp get Red, Green, Blue and Alpha
            {
                case 32:
                    {
                        byte b = Pixels[i];
                        byte g = Pixels[i + 1];
                        byte r = Pixels[i + 2];
                        byte a = Pixels[i + 3]; // a
                        return Color.FromArgb(a, r, g, b);
                    }

                case 24:
                    {
                        byte b = Pixels[i];
                        byte g = Pixels[i + 1];
                        byte r = Pixels[i + 2];
                        return Color.FromArgb(r, g, b);
                    }

                case 8:
                    // For 8 bpp get color value (Red, Green and Blue values are the same)
                    {
                        byte c = Pixels[i];
                        return Color.FromArgb(c, c, c);
                    }

                default:
                    return Color.Empty;
            }
        }

        /// <summary>
        /// Get the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public ArraySegment<byte> GetPixelBytes(int x, int y)
        {
            // Get start index of the specified pixel
            int i = ((y * Width) + x) * colorCount;

            //if (i > Pixels.Length - cCount)
            //    throw new IndexOutOfRangeException();

            switch (Depth) // For 32 bpp get Red, Green, Blue and Alpha
            {
                case 32: return new ArraySegment<byte>(Pixels, i, 4);
                case 24: return new ArraySegment<byte>(Pixels, i, 3);
                case 8: return new ArraySegment<byte>(Pixels, i, 1);
                default:
                    return new ArraySegment<byte>();
            }
        }

        public Color3 GetPixelColor3(int x, int y)
        {
            // Get start index of the specified pixel
            int i = ((y * Width) + x) * colorCount;

            return new Color3(Pixels[i + 2], Pixels[i + 1], Pixels[i]);
        }

        /// <summary>
        /// Set the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void SetPixel(int x, int y, Color color)
        {
            // Get start index of the specified pixel
            int i = ((y * Width) + x) * colorCount;

            switch (Depth) // For 32 bpp set Red, Green, Blue and Alpha
            {
                case 32:
                    Pixels[i] = color.B;
                    Pixels[i + 1] = color.G;
                    Pixels[i + 2] = color.R;
                    Pixels[i + 3] = color.A;
                    break;
                case 24:
                    Pixels[i] = color.B;
                    Pixels[i + 1] = color.G;
                    Pixels[i + 2] = color.R;
                    break;
                case 8:
                    Pixels[i] = color.B;
                    break;
            }
        }

        public void SetPixel(int x, int y, byte[] color)
        {
            // Get start index of the specified pixel
            int i = ((y * Width) + x) * colorCount;

            switch (Depth) // For 32 bpp set Red, Green, Blue and Alpha
            {
                case 32:
                    Pixels[i] = color[0];
                    Pixels[i + 1] = color[1];
                    Pixels[i + 2] = color[2];
                    Pixels[i + 3] = color[3];
                    break;
                case 24:
                    Pixels[i] = color[0];
                    Pixels[i + 1] = color[1];
                    Pixels[i + 2] = color[2];
                    break;
                case 8:
                    Pixels[i] = color[0];
                    break;
            }
        }

        public void SetPixel(int x, int y, Color3 color)
        {
            // Get start index of the specified pixel
            int i = ((y * Width) + x) * colorCount;

            Pixels[i] = color.b;
            Pixels[i + 1] = color.g;
            Pixels[i + 2] = color.r;
        }
    }

    public struct Color3
    {
        public byte r, g, b;
        public Color3(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public Color3(double r, double g, double b)
        {
            this.r = (byte)Math.Round(r);
            this.g = (byte)Math.Round(g);
            this.b = (byte)Math.Round(b);
        }
    }
}
