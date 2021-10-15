using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GrayscaleMaker
{
    public abstract class ColorFilter
    {
        public abstract Color Process(Color from);
    }

    public class EmptyColorFilter : ColorFilter
    {
        public override Color Process(Color from) => from;
    }

    public class GrayscaleColorFilter : ColorFilter
    {
        public override Color Process(Color from)
        {
            var grayValue = (int)Math.Round(((double)from.R + from.G + from.B) / 3d);
            return Color.FromArgb(from.A, grayValue, grayValue, grayValue);
        }
    }

    public class PerceptualGrayscaleColorFilter : ColorFilter
    {
        public override Color Process(Color from) 
        {
            var grayValue = (int)Math.Round(from.R * 0.299 + from.G * 0.587 + from.B * 0.114);
            return Color.FromArgb(from.A, grayValue, grayValue, grayValue);
        }
    }

}
