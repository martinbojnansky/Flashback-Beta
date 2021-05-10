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

namespace VideoEffects
{
    public sealed class XproVideoEffect : IBasicVideoEffect
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
                    WhitePoint = new Vector2(0.99f, 1f)
                };
                var highlightsAndShadows = new HighlightsAndShadowsEffect()
                {
                    Source = brightness,
                    Highlights = 0.1f,
                    Shadows = 0.1f
                };
                var contrast = new ContrastEffect()
                {
                    Source = highlightsAndShadows,
                    Contrast = 1f
                };
                var curves = new LinearTransferEffect()
                {
                    Source = contrast,
                    BlueOffset = 0.176f,
                    BlueSlope = 0.647f
                };
                var vignette = new VignetteEffect()
                {
                    Source = curves,
                    Amount = 0.9f,
                    Curve = 0.15f,
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