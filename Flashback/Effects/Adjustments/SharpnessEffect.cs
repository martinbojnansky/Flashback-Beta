using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using VideoEffects;
using Windows.Foundation.Collections;
using Windows.Media.Editing;
using Windows.Media.Effects;

namespace Flashback.Effects.Adjustments
{
    public class SharpnessEffect : Effect
    {
        public override string Title { get; } = "Sharpness";
        public override string Icon { get; } = "sharpness.png";
        public override Type ControlType { get; } = typeof(IntensityEffectView);
        public override PropertySet Properties { get; set; } = SharpnessVideoEffect.GetDefaultProperties();

        public override void ApplyEffect(MediaClip mediaClip, MediaComposition mediaComposition)
        {
            mediaClip.VideoEffectDefinitions.Add(new VideoEffectDefinition(typeof(SharpnessVideoEffect).FullName, Properties));
        }

        public override void UpdateIsActiveProperty()
        {
            if (Intensity != 0)
                IsActive = true;
            else
                IsActive = false;
        }

        [IgnoreDataMember]
        public double Intensity
        {
            get
            {
                return Convert.ToDouble(Properties[nameof(Intensity)]);
            }
            set
            {
                Properties[nameof(Intensity)] = value;
                RaisePropertyChanged(nameof(Intensity));
            }
        }
    }
}
