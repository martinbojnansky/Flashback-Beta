using System;
using System.Collections.Generic;
using Windows.Foundation.Collections;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;
using Windows.UI;
using Microsoft.Graphics.Canvas.Brushes;

namespace VideoEffects
{
    public sealed class InkwellVideoEffect : IBasicVideoEffect
    {
        private VideoEncodingProperties _currentEncodingProperties;
        private CanvasDevice _canvasDevice;
        private IPropertySet _configuration = GetDefaultProperties();

        public float Intensity
        {
            get
            {
                return Convert.ToSingle(_configuration[nameof(Intensity)]) / 100;
            }
        }

        public void ProcessFrame(ProcessVideoFrameContext context)
        {
            using (CanvasBitmap inputBitmap = CanvasBitmap.CreateFromDirect3D11Surface(_canvasDevice, context.InputFrame.Direct3DSurface))
            using (CanvasRenderTarget renderTarget = CanvasRenderTarget.CreateFromDirect3D11Surface(_canvasDevice, context.OutputFrame.Direct3DSurface))
            using (CanvasDrawingSession ds = renderTarget.CreateDrawingSession())
            {
                var brightness = new BrightnessEffect()
                {
                    Source = inputBitmap,
                    WhitePoint = new Vector2(0.90f, 1f)
                };
                var highlightsAndShadows = new HighlightsAndShadowsEffect()
                {
                    Source = brightness,
                    Highlights = 0.5f,
                    Shadows = -0.5f
                };
                var contrast = new ContrastEffect()
                {
                    Source = highlightsAndShadows,
                    Contrast = 0.8f
                };
                var saturation = new SaturationEffect()
                {
                    Source = highlightsAndShadows,
                    Saturation = 0f
                };
                var contrast2 = new ContrastEffect()
                {
                    Source = saturation,
                    Contrast = 0.6f
                };
                var vignette = new VignetteEffect()
                {
                    Source = contrast2,
                    Amount = 0.9f,
                    Curve = 0.19f,
                    Color = Colors.Black
                };

                ds.DrawImage(vignette);
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
                new KeyValuePair<string, object>(nameof(Intensity), (double)0)
            };
        }
    }
}