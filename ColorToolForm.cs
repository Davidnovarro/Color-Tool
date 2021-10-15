using GrayscaleMaker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrayscaleMaker
{
    public partial class ColorToolForm : Form
    {

        Bitmap bmp;
        Bitmap filteredBmp;
        LockBitmap lockBmp;
        private int topPadding;
        private int rightPadding;
        private int savedImagesCount = 0;

        private ColorFilter Filter = new EmptyColorFilter();

        private readonly string[] filterNames = new string[]
        {
            "None",
            "Grayscale",
            "Grayscale Perceptual"
        };

        public ColorToolForm()
        {
            InitializeComponent();
            this.AllowTransparency = true;
            Rectangle screenRectangle = this.RectangleToScreen(this.ClientRectangle);
            topPadding = screenRectangle.Top - this.Top;
            rightPadding = screenRectangle.Left - this.Left;
            FilterComboBox.DataSource = filterNames;
            FilterComboBox.SelectedIndex = 2;
        }


        private void CaptureScreen()
        {
            this.Opacity = 0f;
            Graphics myGraphics = this.CreateGraphics();
            Size s = this.Size;
            s.Height -= topPadding;
            s.Width -= rightPadding;
            if (bmp == null || bmp.Width != s.Width || bmp.Height != s.Height)
            {
                bmp = new Bitmap(s.Width, s.Height, myGraphics);
            }
            Graphics memoryGraphics = Graphics.FromImage(bmp);
            memoryGraphics.CopyFromScreen(this.Location.X + rightPadding, this.Location.Y + topPadding, 0, 0, s, CopyPixelOperation.MergeCopy);
        }


        private void ApplyFilter()
        {
            if (bmp == null)
                return;

            filteredBmp = (Bitmap)bmp.Clone();
            lockBmp = new LockBitmap(filteredBmp);
            this.Opacity = 1f;

            lockBmp.LockBits();
            for (int y = 0; y < lockBmp.Height; y++)
            {
                for (int x = 0; x < lockBmp.Width; x++)
                {
                    lockBmp.SetPixel(x, y, Filter.Process(lockBmp.GetPixel(x, y)));
                    //var color = lockBmp.GetPixel(x, y);
                    //var grayValue = (int)Math.Round(color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
                    //lockBmp.SetPixel(x, y, Color.FromArgb(255, grayValue, grayValue, grayValue));
                }
            }
            lockBmp.UnlockBits();
            this.BackgroundImage = filteredBmp;
        }

        private void OnCaptureClick(object sender, EventArgs e)
        {
            CaptureScreen();
            ApplyFilter();
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            if (filteredBmp == null)
                return;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Images|*.png;*.jpg";
            dialog.FileName = $"image_{savedImagesCount}.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(dialog.FileName);
                switch (ext)
                {
                    case ".jpg":
                        filteredBmp.Save(dialog.FileName, ImageFormat.Jpeg);
                        break;
                    case ".png":
                        filteredBmp.Save(dialog.FileName, ImageFormat.Png);
                        break;
                }
                savedImagesCount++;
            }
        }

        private void FilterComboBoxChanged(object sender, EventArgs e)
        {
            switch (filterNames[FilterComboBox.SelectedIndex])
            {
                case "Grayscale": Filter = new GrayscaleColorFilter(); break;
                case "Grayscale Perceptual": Filter = new PerceptualGrayscaleColorFilter(); break;
                case "None":
                default: Filter = new EmptyColorFilter(); break;
            }
            ApplyFilter();
        }
    }
}