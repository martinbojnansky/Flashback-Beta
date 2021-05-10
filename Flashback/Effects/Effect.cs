using Flashback.Effects.Adjustments;
using Flashback.Effects.Borders;
using Flashback.Effects.Filters;
using Flashback.Effects.Titles;
using Helpers.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Media.Editing;
using Windows.UI.Xaml.Controls;

namespace Flashback.Effects
{
    [DataContract]
    // Adjustments
    [KnownType(typeof(BrightnessEffect))]
    [KnownType(typeof(ContrastEffect))]
    [KnownType(typeof(SaturationEffect))]
    [KnownType(typeof(HighlightsAndShadowsEffect))]
    [KnownType(typeof(TemperatureAndTintEffect))]
    [KnownType(typeof(SharpnessEffect))]
    [KnownType(typeof(FlareEffect))]
    //[KnownType(typeof(StabilizationEffect))]
    // Effects
    [KnownType(typeof(NormalEffect))]
    //[KnownType(typeof(AutoFixEffect))]
    [KnownType(typeof(BoostEffect))]
    [KnownType(typeof(MonoEffect))]
    [KnownType(typeof(XproEffect))]
    [KnownType(typeof(InkwellEffect))]
    [KnownType(typeof(HipsterEffect))]
    [KnownType(typeof(DreamEffect))]
    // Borders
    [KnownType(typeof(VignetteEffect))]
    // Titles
    [KnownType(typeof(OpeningTitlesEffect))]
    [KnownType(typeof(ClosingTitlesEffect))]
    [KnownType(typeof(FadeInEffect))]
    [KnownType(typeof(FadeOutEffect))]

    public abstract class Effect : INotifyPropertyChanged
    {
        [IgnoreDataMember]
        public abstract string Title { get; }
        [IgnoreDataMember]
        public abstract string Icon { get; }
        [IgnoreDataMember]
        public abstract Type ControlType { get; }
        [DataMember]
        public abstract PropertySet Properties { get; set; }
        [IgnoreDataMember]
        private bool _isActive = false;
        [DataMember]
        public virtual bool IsActive
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

        /// <summary>
        /// Applies effect to media clip and composition.
        /// </summary>
        /// <param name="mediaClip"></param>
        /// <param name="mediaComposition"></param>
        public abstract void ApplyEffect(MediaClip mediaClip, MediaComposition mediaComposition);

        /// <summary>
        /// Checks condition and updates activity indicator
        /// </summary>
        public abstract void UpdateIsActiveProperty();

        /// <summary>
        /// Creates new user control from specified type to manage effect properties
        /// </summary>
        /// <returns></returns>
        public UserControl GetControl()
        {
            // Create user control and pass effect as data context
            var userControl = (UserControl)Activator.CreateInstance(ControlType);
            userControl.DataContext = this;
            return userControl;
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual async void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (!propertyName.Equals(nameof(IsActive)))
            {
                UpdateIsActiveProperty();
            }
            else
            {
                // Updates preview in effects view if activation property has changed
                if (Views.EffectsView.Current != null)
                    await Views.EffectsView.Current.UpdatePreview();
            }
        }

        #endregion
    }
}
