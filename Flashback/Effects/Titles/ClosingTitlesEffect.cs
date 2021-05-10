using System;
using System.Runtime.Serialization;
using VideoEffects;
using Windows.Foundation.Collections;
using Windows.Media.Editing;
using Windows.Media.Effects;
using Windows.UI;

namespace Flashback.Effects.Titles
{
    [KnownType(typeof(Color))]
    public class ClosingTitlesEffect : Effect
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
        public override string Title { get; } = "Closing Titles";
        public override string Icon { get; }
        public override Type ControlType { get; } = null;
        public override PropertySet Properties { get; set; } = TextVideoCompositor.GetDefaultProperties();

        public override void ApplyEffect(MediaClip mediaClip, MediaComposition mediaComposition)
        {
            if (string.IsNullOrEmpty(Text))
                return;
        
            mediaClip = MediaClip.CreateFromColor(Colors.Black, TimeSpan.FromSeconds(3));
            mediaComposition.Clips.Add(mediaClip);
            Properties["StartTime"] = mediaClip.StartTimeInComposition.TotalSeconds;
            mediaClip.VideoEffectDefinitions.Add(new VideoEffectDefinition(typeof(TextVideoEffect).FullName, Properties));
        }

        public override void UpdateIsActiveProperty() { }

        [IgnoreDataMember]
        public string Text
        {
            get
            {
                return (string)Properties[nameof(Text)];
            }
            set
            {
                Properties[nameof(Text)] = value;
                RaisePropertyChanged(nameof(Text));
            }
        }
        
        [IgnoreDataMember]
        public bool Fade
        {
            get
            {
                return (bool)Properties[nameof(Fade)];
            }
            set
            {
                Properties[nameof(Fade)] = value;
                RaisePropertyChanged(nameof(Fade));
            }
        }
    }
}
