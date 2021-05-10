using System;
using VideoEffects;
using Windows.Foundation.Collections;
using Windows.Media.Editing;
using Windows.Media.Effects;

namespace Flashback.Effects.Filters
{
    public class HipsterEffect : Effect
    {
        public override string Title { get; } = "Hipster";
        public override string Icon { get; } = "Hipster.png";
        public override Type ControlType { get; } = typeof(Windows.UI.Xaml.Controls.UserControl);
        public override PropertySet Properties { get; set; }

        public override void ApplyEffect(MediaClip mediaClip, MediaComposition mediaComposition)
        {
            mediaClip.VideoEffectDefinitions.Add(new VideoEffectDefinition(typeof(HipsterVideoEffect).FullName));
        }

        public override void UpdateIsActiveProperty() { }
    }
}