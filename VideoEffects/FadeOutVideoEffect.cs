using System;
using System.Collections.Generic;
using Windows.Foundation.Collections;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI;

namespace VideoEffects
{
    public sealed class FadeOutVideoEffect : IBasicVideoEffect
    {
        private VideoEncodingProperties _currentEncodingProperties;
        private CanvasDevice _canvasDevice;
        private IPropertySet _configuration = GetDefaultProperties();

        public TimeSpan Duration
        {
            get
            {
                return TimeSpan.FromSeconds((double)_configuration[nameof(Duration)]);
            }
        }

        public TimeSpan EndTime
        {
            get
            {
                return TimeSpan.FromSeconds((double)_configuration[nameof(EndTime)]);
            }
        }

        public void ProcessFrame(ProcessVideoFrameContext context)
        {
            using (CanvasBitmap inputBitmap = CanvasBitmap.CreateFromDirect3D11Surface(_canvasDevice, context.InputFrame.Direct3DSurface))
            using (CanvasRenderTarget renderTarget = CanvasRenderTarget.CreateFromDirect3D11Surface(_canvasDevice, context.OutputFrame.Direct3DSurface))
            using (CanvasDrawingSession ds = renderTarget.CreateDrawingSession())
            {
                TimeSpan time = context.InputFrame.RelativeTime.HasValue ? context.InputFrame.RelativeTime.Value : new TimeSpan();
                TimeSpan start = EndTime - Duration;

                if (time >= start)
                {
                    Byte alpha = 255;

                    if (time <= EndTime)
                    {
                        var alphaString = ((int)(((time.TotalSeconds - start.TotalSeconds) / Duration.TotalSeconds) * 255)).ToString();
                        Byte.TryParse(alphaString, out alpha);
                    }

                    var composite = new CompositeEffect();
                    composite.Sources.Add(inputBitmap);
                    composite.Sources.Add(new ColorSourceEffect() { Color = new Color() { A = alpha, R = 0, G = 0, B = 0 } });

                    ds.DrawImage(composite);
                }
                else
                {
                    ds.DrawImage(inputBitmap);
                }
            }       
        }

        public void SetEncodingProperties(VideoEncodingProperties encodingProperties, IDirect3DDevice device)
        {
            _currentEncodingProperties = encodingProperties;
            _canvasDevice = CanvasDevice.CreateFromDirect3D11Device(device);
            CanvasDevice.DebugLevel = CanvasDebugLevel.Error;
        }

        public void SetProperties(IPropertySet configuration)
        {
            _configuration = configuration;
        }

        public bool IsReadOnly { get { return false; } }
        public MediaMemoryTypes SupportedMemoryTypes { get { return MediaMemoryTypes.Gpu; } }
        public bool TimeIndependent { get { return false; } }

        public IReadOnlyList<VideoEncodingProperties> SupportedEncodingProperties
        {
            get
            {
                return new List<VideoEncodingProperties>()
                {
                    // NOTE: Specifying width and height is only necessary due to bug in media pipeline when
                    // effect is being used with Media Capture. 
                    // This can be changed to "0, 0" in a future release of FBL_IMPRESSIVE. 
                    VideoEncodingProperties.CreateUncompressed(MediaEncodingSubtypes.Argb32, 800, 600)
                };
            }
        }

        public void Close(MediaEffectClosedReason reason)
        {
            // Clean up devices
            if (_canvasDevice != null)
                _canvasDevice.Dispose();
        }

        public void DiscardQueuedFrames()
        {
            // No cached frames to discard
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PropertySet GetDefaultProperties()
        {
            return new PropertySet()
            {
                new KeyValuePair<string, object>(nameof(Duration), 2.0),
                new KeyValuePair<string, object>(nameof(EndTime), 0.0),
            };
        }
    }
}