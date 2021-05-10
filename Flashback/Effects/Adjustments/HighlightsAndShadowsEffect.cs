using System;
using System.Runtime.Serialization;
using VideoEffects;
using Windows.Foundation.Collections;
using Windows.Media.Editing;
using Windows.Media.Effects;

namespace Flashback.Effects.Adjustments
{
    public class HighlightsAndShadowsEffect : Effect
    {
        public override string Title { get; } = "Highlights & Shadows";
        public override string Icon { get; } = "HighlightsAndShadows.png";
        public override Type ControlType { get; } = typeof(HighlightsAndShadowsEffectView);
        public override PropertySet Properties { get; set; } = HighlightsAndShadowsVideoEffect.GetDefaultProperties();

        public override void ApplyEffect(MediaClip mediaClip, MediaComposition mediaComposition)
        {
            mediaClip.VideoEffectDefinitions.Add(new VideoEffectDefinition(typeof(HighlightsAndShadowsVideoEffect).FullName, Properties));
        }

        public override void UpdateIsActiveProperty()
        {
            if (Highlights != 0 || Shadows != 0)
                IsActive = true;
            else
                IsActive = false;
        }

        [IgnoreDataMember]
        public double Highlights
        {
            get
            {
                return Convert.ToDouble(Properties[nameof(Highlights)]);
            }
            set
            {
                Properties[nameof(Highlights)] = value;
                RaisePropertyChanged(nameof(Highlights));
            }
        }

        [IgnoreDataMember]
        public double Shadows
        {
            get
            {
                return Convert.ToDouble(Properties[nameof(Shadows)]);
            }
            set
            {
                Properties[nameof(Shadows)] = value;
                RaisePropertyChanged(nameof(Shadows));
            }
        }
    }
}
