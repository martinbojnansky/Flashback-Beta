using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Helpers.Controls
{
    public sealed partial class RangeSlider : UserControl
    {
        public RangeSlider()
        {
            this.InitializeComponent();
        }

        #region Depencies

        public double MinimumRange
        {
            get { return (double)GetValue(MinimumRangeProperty); }
            set { SetValue(MinimumRangeProperty, value); }
        }

        public static readonly DependencyProperty MinimumRangeProperty =
              DependencyProperty.Register(
                  nameof(MinimumRange), typeof(double), typeof(RangeSlider), new PropertyMetadata(1.0)
                  );

        public double Start
        {
            get { return (double)GetValue(StartProperty); }
            set { SetValue(StartProperty, value); }
        }

        public static readonly DependencyProperty StartProperty =
              DependencyProperty.Register(
                  nameof(Start), typeof(double), typeof(RangeSlider), new PropertyMetadata(MinimumProperty)
                  );

        public double End
        {
            get { return (double)GetValue(EndProperty); }
            set { SetValue(EndProperty, value); }
        }

        public static readonly DependencyProperty EndProperty =
              DependencyProperty.Register(
                  nameof(End), typeof(double), typeof(RangeSlider), new PropertyMetadata(MaximumProperty)
                  );

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty =
              DependencyProperty.Register(
                  nameof(Minimum), typeof(double), typeof(RangeSlider), new PropertyMetadata(0.0)
                  );

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty =
              DependencyProperty.Register(
                  nameof(Maximum), typeof(double), typeof(RangeSlider), new PropertyMetadata(100.0)
                  );

        public double StepFrequency
        {
            get { return (double)GetValue(StepFrequencyProperty); }
            set { SetValue(StepFrequencyProperty, value); }
        }

        public static readonly DependencyProperty StepFrequencyProperty =
              DependencyProperty.Register(
                  nameof(StepFrequency), typeof(double), typeof(RangeSlider), new PropertyMetadata(1.0)
                  );

        public bool IsStartSliderEnabled
        {
            get { return (bool)GetValue(IsStartSliderEnabledProperty); }
            set { SetValue(IsStartSliderEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsStartSliderEnabledProperty =
              DependencyProperty.Register(
                  nameof(IsStartSliderEnabled), typeof(bool), typeof(RangeSlider), new PropertyMetadata(true)
                  );

        public bool IsEndSliderEnabled
        {
            get { return (bool)GetValue(IsEndSliderEnabledProperty); }
            set { SetValue(IsEndSliderEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsEndSliderEnabledProperty =
              DependencyProperty.Register(
                  nameof(IsEndSliderEnabled), typeof(bool), typeof(RangeSlider), new PropertyMetadata(true)
                  );

        public double TickFrequency
        {
            get { return (double)GetValue(TickFrequencyProperty); }
            set { SetValue(TickFrequencyProperty, value); }
        }

        public static readonly DependencyProperty TickFrequencyProperty =
              DependencyProperty.Register(
                  nameof(TickFrequency), typeof(double), typeof(RangeSlider), new PropertyMetadata(.0)
                  );

        public IValueConverter ThumbTooltipValueConverter
        {
            get { return (IValueConverter)GetValue(ThumbTooltipValueConverterProperty); }
            set { SetValue(ThumbTooltipValueConverterProperty, value); }
        }

        public static readonly DependencyProperty ThumbTooltipValueConverterProperty =
              DependencyProperty.Register(
                  nameof(ThumbTooltipValueConverter), typeof(IValueConverter), typeof(RangeSlider), new PropertyMetadata(null)
                  );

        #endregion

        /// <summary>
        /// Restricts maximum value for start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var maximum = End - MinimumRange;
            if (e.NewValue > maximum)
            {
                StartSlider.Value = maximum;
            } 
        }

        /// <summary>
        /// Restricts minimum value for end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var minimum = Start + MinimumRange;
            if (e.NewValue < minimum)
            {
                EndSlider.Value = minimum;
            }
        }
    }
}
