using System;
using System.Runtime.Serialization;
using VideoEffects;
using Windows.Foundation.Collections;
using Windows.Media.Editing;
using Windows.Media.Effects;

namespace Flashback.Effects.Adjustments
{
    public class TemperatureAndTintEffect : Effect
    {
        public override string Title { get; } = "Temperature & Tint";
        public override string Icon { get; } = "TemperatureAndTint.png";
        public override Type ControlType { get; } = typeof(TemperatureAndTintEffectView);
        public override PropertySet Properties { get; set; } = TemperatureAndTintVideoEffect.GetDefaultProperties();

        public override void ApplyEffect(MediaClip mediaClip, MediaComposition mediaComposition)
        {
            mediaClip.VideoEffectDefinitions.Add(new VideoEffectDefinition(typeof(TemperatureAndTintVideoEffect).FullName, Properties));
        }

        public override void UpdateIsActiveProperty()
        {
            if (Temperature != 0 || Tint != 0)
                IsActive = true;
            else
                IsActive = false;
        }

        [IgnoreDataMember]
        public double Temperature
        {
            get
            {
                return Convert.ToDouble(Properties[nameof(Temperature)]);
            }
            set
            {
                Properties[nameof(Temperature)] = value;
                RaisePropertyChanged(nameof(Temperature));
            }
        }

        [IgnoreDataMember]
        public double Tint
        {
            get
            {
                return Convert.ToDouble(Properties[nameof(Tint)]);
            }
            set
            {
                Properties[nameof(Tint)] = value;
                RaisePropertyChanged(nameof(Tint));
            }
        }
    }
}
