using System;
using System.Runtime.Serialization;
using VideoEffects;
using Windows.Foundation.Collections;
using Windows.Media.Editing;
using Windows.Media.Effects;

namespace Flashback.Effects.Adjustments
{
    public class ContrastEffect : Effect
    {
        public override string Title { get; } = "Contrast";
        public override string Icon { get; } = "Contrast.png";
        public override Type ControlType { get; } = typeof(TwoWayIntensityEffectView);
        public override PropertySet Properties { get; set; } = ContrastVideoEffect.GetDefaultProperties();

        public override void ApplyEffect(MediaClip mediaClip, MediaComposition mediaComposition)
        {
            mediaClip.VideoEffectDefinitions.Add(new VideoEffectDefinition(typeof(ContrastVideoEffect).FullName, Properties));
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
