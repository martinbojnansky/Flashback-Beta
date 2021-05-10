using Flashback.Views;
using System;
using System.Runtime.Serialization;
using VideoEffects;
using Windows.Foundation.Collections;
using Windows.Media.Editing;
using Windows.Media.Effects;

namespace Flashback.Effects.Adjustments
{
    public class BrightnessEffect : Effect
    {
        public override string Title { get; } = "Brightness";
        public override string Icon { get; } = "Brightness.png";
        public override Type ControlType { get; } = typeof(TwoWayIntensityEffectView);
        public override PropertySet Properties { get; set; } = BrightnessVideoEffect.GetDefaultProperties();

        public override void ApplyEffect(MediaClip mediaClip, MediaComposition mediaComposition)
        {
            mediaClip.VideoEffectDefinitions.Add(new VideoEffectDefinition(typeof(BrightnessVideoEffect).FullName, Properties));
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
