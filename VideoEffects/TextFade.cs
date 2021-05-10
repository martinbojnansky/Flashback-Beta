using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEffects
{
    public sealed class TextFade
    {
        public TextFade(TimeSpan duration, TimeSpan fadeInDuration, TimeSpan fadeOutDuration, TimeSpan startOffest)
        {
            Duration = duration;
            FadeInDuration = fadeInDuration;
            FadeOutDuration = fadeOutDuration;
            StartOffset = startOffest;
        }

        public TimeSpan StartOffset { get; set; } = TimeSpan.FromSeconds(0.5);

        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(3);

        public TimeSpan FadeInDuration { get; set; } = TimeSpan.FromSeconds(0);

        public TimeSpan FadeOutDuration { get; set; } = TimeSpan.FromSeconds(0);

        /// <summary>
        /// Gets opacity value based on relative time
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public byte GetAplha(TimeSpan time)
        {
            byte alpha = 0;

            if (time >= StartOffset && time <= StartOffset + FadeInDuration + Duration + FadeOutDuration)
            {
                alpha = 255;
                TimeSpan fadeOutStart;

                if (time <= (StartOffset + FadeInDuration))
                {
                    // FadeIn
                    var alphaString = ((int)(((time - StartOffset).TotalSeconds / FadeInDuration.TotalSeconds) * 255)).ToString();
                    byte.TryParse(alphaString, out alpha);
                }
                else if (time >= (fadeOutStart = (StartOffset + FadeInDuration + Duration)))
                {
                    // FadeOut
                    var alphaString = ((int)(255 - (((time - fadeOutStart).TotalSeconds / FadeOutDuration.TotalSeconds) * 255))).ToString();
                    byte.TryParse(alphaString, out alpha);
                }
            }

            return alpha;
        }
    }
}
