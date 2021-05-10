using System;
using System.Runtime.Serialization;
using VideoEffects;
using Windows.Foundation.Collections;
using Windows.Media.Editing;
using Windows.Media.Effects;

namespace Flashback.Effects.Filters
{
    public class NormalEffect : Effect
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

        public override string Title { get; } = "Normal";
        public override string Icon { get; } = "Normal.png";
        public override Type ControlType { get; } = typeof(Windows.UI.Xaml.Controls.UserControl);
        public override PropertySet Properties { get; set; }

        public override void ApplyEffect(MediaClip mediaClip, MediaComposition mediaComposition) { }

        public override void UpdateIsActiveProperty() { }
    }
}
