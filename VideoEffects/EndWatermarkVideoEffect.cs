using System;
using System.Collections.Generic;
using Windows.Foundation.Collections;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;
using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.UI;
using Microsoft.Graphics.Canvas.Text;

namespace VideoEffects
{
    public sealed class EndWatermarkVideoEffect : IBasicVideoEffect
    {
        private VideoEncodingProperties _currentEncodingProperties;
        private CanvasDevice _canvasDevice;
        private IPropertySet _configuration = GetDefaultProperties();
        private CroppedBounds _croppedBounds;

        public bool Fade
        {
            get
            {
                return (bool)_configuration[nameof(Fade)];
            }
        }

        public Color Color
        {
            get
            {
                return (Color)_configuration[nameof(Color)];
            }
        }

        public TimeSpan StartTime
        {
            get
            {
                return TimeSpan.FromSeconds((double)_configuration[nameof(StartTime)]);
            }
        }

        public void ProcessFrame(ProcessVideoFrameContext context)
        {
            using (CanvasBitmap inputBitmap = CanvasBitmap.CreateFromDirect3D11Surface(_canvasDevice, context.InputFrame.Direct3DSurface))
            using (CanvasRenderTarget renderTarget = CanvasRenderTarget.CreateFromDirect3D11Surface(_canvasDevice, context.OutputFrame.Direct3DSurface))
            using (CanvasDrawingSession ds = renderTarget.CreateDrawingSession())
            {
                ds.DrawImage(inputBitmap);

                TimeSpan time = context.OutputFrame.RelativeTime.HasValue ? context.OutputFrame.RelativeTime.Value : new TimeSpan();

                TextFade textFade;
                Color color = Color;

                if (Fade)
                {
                    textFade = new TextFade(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), StartTime);
                    var alpha = textFade.GetAplha(time);
                    if (alpha == 0) return;
                    color.A = alpha;
                }

                if (_croppedBounds == null)
                    _croppedBounds = new CroppedBounds(renderTarget.Bounds);

                ds.DrawText("    Ö Flashback", _croppedBounds.Center, color,
                        new CanvasTextFormat()
                        {
                            FontFamily = "ms-appx:///Assets/Fonts/Flashback.ttf#Signika",
                            FontSize = Convert.ToSingle(0.06 * _croppedBounds.CroppedHeight),
                            HorizontalAlignment = CanvasHorizontalAlignment.Center,
                            VerticalAlignment = CanvasVerticalAlignment.Center
                        });
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
                new KeyValuePair<string, object>(nameof(Fade), false),
                new KeyValuePair<string, object>(nameof(Color), Colors.White),
                new KeyValuePair<string, object>(nameof(StartTime), 0.0)
            };
        }
    }
}