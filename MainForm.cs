using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ColorTool
{
    public partial class MainForm : Form
    {

        Bitmap bmp;
        LockBitmap lockBmp;
        private int topPadding;
        private int rightPadding;
        private int savedImagesCount = 0;

        private ImageFilter[] Filters = new ImageFilter[2] { new EmptyFilter(), new EmptyFilter() };

        private const string NONE = "None";
        private const string GRAYSCALE = "Grayscale";
        private const string GRAYSCALE_PERCEPTUAL = "Grayscale Perceptual";
        private const string BLUR_5PX = "Blur 5px";
        private const string BLUR_10PX = "Blur 10px";
        private const string BLUR_15PX = "Blur 15px";
        private const string BLUR_20PX = "Blur 20px";
        private const string BLUR_30PX = "Blur 30px";
        private const string SCALE_DOWN_2X = "Scale Down 2X";
        private const string SCALE_DOWN_4X = "Scale Down 4X";
        private const string SCALE_DOWN_10X = "Scale Down 10X";

        private readonly string[] filterNames = new string[]
        {
            NONE,
            GRAYSCALE,
            GRAYSCALE_PERCEPTUAL,
            BLUR_5PX,
            BLUR_10PX,
            BLUR_15PX,
            BLUR_20PX,
            BLUR_30PX,
            SCALE_DOWN_2X,
            SCALE_DOWN_4X,
            SCALE_DOWN_10X
        };

        public MainForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += OnKeyDown;
            this.AllowTransparency = true;
            Rectangle screenRectangle = this.RectangleToScreen(this.ClientRectangle);
            topPadding = screenRectangle.Top - this.Top;
            rightPadding = screenRectangle.Left - this.Left;
            FilterComboBox_0.DataSource = filterNames.Clone();
            FilterComboBox_0.SelectedIndex = 2;
            FilterComboBox_1.DataSource = filterNames.Clone();
            FilterComboBox_1.SelectedIndex = 0;
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

            lockBmp = new LockBitmap((Bitmap)bmp.Clone());
            this.Opacity = 1f;
            foreach (var f in Filters)
            {
                f.Process(ref lockBmp);
            }
            lockBmp.UnlockBits();
            this.BackgroundImage = lockBmp.source;
        }

        private void OnCaptureClick(object sender, EventArgs e)
        {
            CaptureScreen();
            ApplyFilter();
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            if (lockBmp == null || lockBmp.source == null)
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
                        lockBmp.source.Save(dialog.FileName, ImageFormat.Jpeg);
                        break;
                    case ".png":
                        lockBmp.source.Save(dialog.FileName, ImageFormat.Png);
                        break;
                }
                savedImagesCount++;
            }
        }

        private void OnFilter0_Changed(object sender, EventArgs e)
        {
            SetFilter(ref Filters[0], filterNames[FilterComboBox_0.SelectedIndex]);
            ApplyFilter();
        }

        private void OnFilter1_Changed(object sender, EventArgs e)
        {
            SetFilter(ref Filters[1], filterNames[FilterComboBox_1.SelectedIndex]);
            ApplyFilter();
        }

        private void SetFilter(ref ImageFilter filter, string filterName)
        {
            switch (filterName)
            {
                case GRAYSCALE: filter = new GrayscaleFilter(); break;
                case GRAYSCALE_PERCEPTUAL: filter = new PerceptualGrayscaleFilter(); break;
                case BLUR_5PX: filter = new BlurFilter(5); break;
                case BLUR_10PX: filter = new BlurFilter(10); break;
                case BLUR_15PX: filter = new BlurFilter(15); break;
                case BLUR_20PX: filter = new BlurFilter(20); break;
                case BLUR_30PX: filter = new BlurFilter(30); break;
                case SCALE_DOWN_2X: filter = new ScaleDownFilter(2); break;
                case SCALE_DOWN_4X: filter = new ScaleDownFilter(4); break;
                case SCALE_DOWN_10X: filter = new ScaleDownFilter(10); break;
                case NONE:
                default: filter = new EmptyFilter(); break;
            }
        }


        private void OnKeyDown(object sender, KeyEventArgs key)
        {
            if ((key.KeyCode & Keys.ControlKey) != 0)
            {
                OnCaptureClick(null, null);
            }
        }

        private int ClampFilterIndex(int t) => t > Filters.Length ? 0 : t < 0 ? 0 : t;
    }
}