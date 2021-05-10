using System;
using System.Runtime.Serialization;
using VideoEffects;
using Windows.Foundation.Collections;
using Windows.Media.Editing;
using Windows.Media.Effects;
using Windows.UI;

namespace Flashback.Effects.Titles
{
    public class EndWatermarkEffect : Effect
    {
        [IgnoreDataMember]
        private bool _isActive = true;
        [DataMember]
        public override bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    RaisePropertyChanged(nameof(IsActive));
                }
            }
        }
        public override string Title { get; }
        public override string Icon { get; }
        public override Type ControlType { get; } = null;
        public override PropertySet Properties { get; set; } = EndWatermarkVideoEffect.GetDefaultProperties();

        public override void ApplyEffect(MediaClip mediaClip, MediaComposition mediaComposition)
        {
            var fade = ViewModels.ProjectViewModel.Instance.Project.ClosingTitlesEffect.Fade;
            if (fade)
                mediaClip = MediaClip.CreateFromColor(Colors.Black, TimeSpan.FromSeconds(3));
            else
                mediaClip = MediaClip.CreateFromColor(Colors.Black, TimeSpan.FromSeconds(2));
            mediaComposition.Clips.Add(mediaClip);

            Properties["Fade"] = fade;
            Properties["StartTime"] = mediaClip.StartTimeInComposition.TotalSeconds;
            mediaClip.VideoEffectDefinitions.Add(new VideoEffectDefinition(typeof(EndWatermarkVideoEffect).FullName, Properties));
        }

        public override void UpdateIsActiveProperty() { }
    }
}
