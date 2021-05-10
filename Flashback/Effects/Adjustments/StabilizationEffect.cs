using System;
using VideoEffects;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Media.Effects;

namespace Flashback.Effects.Adjustments
{
    public class StabilizationEffect : Effect
    {
        public override string Title { get; } = "Stabilization";
        public override string Icon { get; } = "Stabilization.png";
        public override Type ControlType { get; } = typeof(SwitchEffectView);
        public override PropertySet Properties { get; set; }

        public override void ApplyEffect(MediaClip mediaClip, MediaComposition mediaComposition)
        {
            mediaClip.VideoEffectDefinitions.Add(new VideoEffectDefinition(typeof(VideoStabilizationEffect).FullName));
        }

        public override void UpdateIsActiveProperty() { }
    }
}