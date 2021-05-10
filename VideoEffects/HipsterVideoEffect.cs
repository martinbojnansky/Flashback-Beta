﻿using System;
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
    public sealed class HipsterVideoEffect : IBasicVideoEffect
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
                var contrast = new ContrastEffect()
                {
                    Source = brightness,
                    Contrast = 0.3f
                };
                var highlightsAndShadows = new HighlightsAndShadowsEffect()
                {
                    Source = contrast,
                    Highlights = 0.20f,
                    Shadows = 0.30f
                };
                var curves = new LinearTransferEffect()
                {
                    Source = highlightsAndShadows,
                    GreenOffset = 0.0725f,
                    BlueOffset = 0.34f
                };
                var colorOverlay = new ColorSourceEffect()
                {
                    Color = Color.FromArgb(255, 247, 217, 173)
                };
                var blend = new BlendEffect()
                {
                    Mode = BlendEffectMode.Multiply,
                    Background = curves,
                    Foreground = colorOverlay
                };
                var contrast2 = new ContrastEffect()
                {
                    Source = blend,
                    Contrast = 0.40f
                };

                ds.DrawImage(contrast2);
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