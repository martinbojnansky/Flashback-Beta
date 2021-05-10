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
    public class OpeningTitlesEffect : Effect
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
        public override string Title { get; } = "Opening Titles";
        public override string Icon { get; }
        public override Type ControlType { get; } = null;
        public override PropertySet Properties { get; set; } = TextVideoCompositor.GetDefaultProperties();

        public override void ApplyEffect(MediaClip mediaClip, MediaComposition mediaComposition)
        {
            if (string.IsNullOrEmpty(Text))
                return;

            if (Fade)
            {
                mediaClip = MediaClip.CreateFromColor(Colors.Transparent, TimeSpan.FromSeconds(3.5)); ;
                Properties["StartTime"] = 0.3;
            }
            else
            {
                mediaClip = MediaClip.CreateFromColor(Colors.Transparent, TimeSpan.FromSeconds(2));
                Properties["StartTime"] = 0.0;
            }

            var overlayLayer = new MediaOverlayLayer(new VideoCompositorDefinition(typeof(TextVideoCompositor).FullName, Properties));
            mediaComposition.OverlayLayers.Add(overlayLayer);           
            overlayLayer.Overlays.Add(new MediaOverlay(mediaClip));
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
