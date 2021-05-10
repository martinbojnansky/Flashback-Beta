using System;
using System.Collections.Generic;
using Windows.Media.Effects;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.MediaProperties;

namespace AudioEffects
{
    public sealed class FadeAudioEffect : IBasicAudioEffect
    {
        private AudioEncodingProperties _currentEncodingProperties;
        private List<AudioEncodingProperties> _supportedEncodingProperties;

        private IPropertySet _configuration;

        private bool IsFadeInEnabled
        {
            get
            {
                return (bool)_configuration[nameof(IsFadeInEnabled)];
            }
        }

        private TimeSpan FadeInDuration
        {
            get
            {
                return TimeSpan.FromSeconds((double)_configuration[nameof(FadeInDuration)]);
            }
        }

        private TimeSpan FadeOutDuration
        {
            get
            {
                return TimeSpan.FromSeconds((double)_configuration[nameof(FadeOutDuration)]);
            }
        }

        private TimeSpan EndTime
        {
            get
            {
                return TimeSpan.FromSeconds((double)_configuration[nameof(EndTime)]);
            }
        }

        public bool UseInputFrameForOutput { get { return false; } }
        public bool TimeIndependent { get { return false; } }
        public bool IsReadyOnly { get { return true; } }

        // Set up constant members in the constructor
        public FadeAudioEffect()
        {
            // Support 44.1kHz and 48kHz mono float
            _supportedEncodingProperties = new List<AudioEncodingProperties>();
            AudioEncodingProperties encodingProps1 = AudioEncodingProperties.CreatePcm(44100, 1, 32);
            encodingProps1.Subtype = MediaEncodingSubtypes.Float;
            AudioEncodingProperties encodingProps2 = AudioEncodingProperties.CreatePcm(48000, 1, 32);
            encodingProps2.Subtype = MediaEncodingSubtypes.Float;

            _supportedEncodingProperties.Add(encodingProps1);
            _supportedEncodingProperties.Add(encodingProps2);
        }

        public IReadOnlyList<AudioEncodingProperties> SupportedEncodingProperties
        {
            get
            {
                return _supportedEncodingProperties;
            }
        }

        public void SetEncodingProperties(AudioEncodingProperties encodingProperties)
        {
            _currentEncodingProperties = encodingProperties;
        }

        unsafe public void ProcessFrame(ProcessAudioFrameContext context)
        {
            AudioFrame inputFrame = context.InputFrame;
            AudioFrame outputFrame = context.OutputFrame;

            using (AudioBuffer inputBuffer = inputFrame.LockBuffer(AudioBufferAccessMode.Read),
                            outputBuffer = outputFrame.LockBuffer(AudioBufferAccessMode.Write))
            using (IMemoryBufferReference inputReference = inputBuffer.CreateReference(),
                                            outputReference = outputBuffer.CreateReference())
            {
                byte* inputDataInBytes;
                byte* outputDataInBytes;
                uint inputCapacity;
                uint outputCapacity;

                ((IMemoryBufferByteAccess)inputReference).GetBuffer(out inputDataInBytes, out inputCapacity);
                ((IMemoryBufferByteAccess)outputReference).GetBuffer(out outputDataInBytes, out outputCapacity);

                float* inputDataInFloat = (float*)inputDataInBytes;
                float* outputDataInFloat = (float*)outputDataInBytes;

                float inputData;

                // Process audio data
                int dataInFloatLength = (int)inputBuffer.Length / sizeof(float);

                TimeSpan relativeTime = inputFrame.RelativeTime.HasValue ? inputFrame.RelativeTime.Value : new TimeSpan();
                TimeSpan frameDuration = inputFrame.Duration.HasValue ? inputFrame.Duration.Value : new TimeSpan();
                var stepDurationInSeconds = frameDuration.TotalSeconds / dataInFloatLength;

                for (int i = 0; i < dataInFloatLength; i++)
                {
                    TimeSpan time = relativeTime + TimeSpan.FromSeconds(i * stepDurationInSeconds);
                    TimeSpan fadeOutStart;

                    // Fade in
                    if (time < FadeInDuration)
                    {
                        if (IsFadeInEnabled)
                        {
                            var x = time.TotalSeconds / FadeInDuration.TotalSeconds * 0.97;
                            var gain = Convert.ToSingle(Math.Pow(x + 0.03, 2));
                            inputData = inputDataInFloat[i] * gain;
                        }
                        else
                            inputData = inputDataInFloat[i];
                    }
                    // Normal
                    else if (time < (fadeOutStart = EndTime - FadeOutDuration))
                    {
                        inputData = inputDataInFloat[i];
                    }
                    // Fade out
                    else if (time < EndTime)
                    {
                        var x = 1 - ((time - fadeOutStart).TotalSeconds / FadeOutDuration.TotalSeconds);
                        var gain = Convert.ToSingle(Math.Pow(x, 2));
                        inputData = inputDataInFloat[i] * gain;
                    }
                    else
                        inputData = 0f;
                    
                    outputDataInFloat[i] = inputData;
                }
            }
        }

        public void Close(MediaEffectClosedReason reason)
        {
            // Clean-up any effect resources
            // This effect doesn't care about close, so there's nothing to do
        }

        public void DiscardQueuedFrames()
        {
        }

        public void SetProperties(IPropertySet configuration)
        {
            this._configuration = configuration;
        }

        public static PropertySet GetDefaultProperties()
        {
            return new PropertySet()
            {
                new KeyValuePair<string, object>(nameof(FadeInDuration), 3.0),
                new KeyValuePair<string, object>(nameof(IsFadeInEnabled), false),
                new KeyValuePair<string, object>(nameof(FadeOutDuration), 5.0),
                new KeyValuePair<string, object>(nameof(EndTime), 3600.0)
            };
        }
    }
}