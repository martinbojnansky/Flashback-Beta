using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace VideoEffects
{
    public sealed class CroppedBounds
    {
        public CroppedBounds(Rect bounds)
        {
            CroppedWidth = bounds.Width;
            Scale = CroppedWidth / 16;
            CroppedHeight = 9 * Scale;
            Top = (bounds.Height - CroppedHeight) / 2;
            Bottom = Top + CroppedHeight;
            Center = new Vector2(Convert.ToSingle(CroppedWidth / 2), Convert.ToSingle(Top + CroppedHeight / 2));
        }

        public double CroppedWidth { get; set; }
        public double CroppedHeight { get; set; }
        public double Scale { get; set; }
        public double Top { get; set; }
        public Vector2 Center { get; set; }
        public double Bottom { get; set; }
    }
}
