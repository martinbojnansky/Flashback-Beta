using System;
using VideoEffects;
using Windows.Foundation.Collections;
using Windows.Media.Editing;
using Windows.Media.Effects;

namespace Flashback.Effects.Filters
{
    public class CartoonEffect : Effect
    {
        public override string Title { get; } = "Cartoon";
        public override string Icon { get; } = "Cartoon.png";
        public override Type ControlType { get; } = typeof(Windows.UI.Xaml.Controls.UserControl);
        public override PropertySet Properties { get; set; }

        public override void ApplyEffect(MediaClip mediaClip, MediaComposition mediaComposition)
        {
            mediaClip.VideoEffectDefinitions.Add(new VideoEffectDefinition(typeof(CartoonVideoEffect).FullName));
        }

        public override void UpdateIsActiveProperty() { }
    }
}