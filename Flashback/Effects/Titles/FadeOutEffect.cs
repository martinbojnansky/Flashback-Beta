﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using VideoEffects;
using Windows.Foundation.Collections;
using Windows.Media.Editing;
using Windows.Media.Effects;

namespace Flashback.Effects.Titles
{
    public class FadeOutEffect : Effect
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
        public override string Title { get; } = "Fade Out";
        public override string Icon { get; } = "FadeOut.png";
        public override Type ControlType { get; }
        public override PropertySet Properties { get; set; } = FadeOutVideoEffect.GetDefaultProperties();

        public override void ApplyEffect(MediaClip mediaClip, MediaComposition mediaComposition)
        {
            if (!IsEnabled)
                return;

            var duration = mediaClip.TrimmedDuration.TotalSeconds;
            if (duration < 4)
                Properties["Duration"] = duration / 2;
            else
                Properties["Duration"] = 2.0;
            Properties["EndTime"] = mediaClip.EndTimeInComposition.TotalSeconds;
            mediaClip.VideoEffectDefinitions.Add(new VideoEffectDefinition(typeof(FadeOutVideoEffect).FullName, Properties));
        }

        [IgnoreDataMember]
        private bool _isEnabled;
        [DataMember]
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged(nameof(IsEnabled));
            }
        }

        public override void UpdateIsActiveProperty() { }
    }
}
