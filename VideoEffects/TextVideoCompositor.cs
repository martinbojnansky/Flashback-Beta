using System;
using Windows.Foundation.Collections;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;
using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.UI.Text;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Text;
using Windows.UI;
using System.Collections.Generic;

namespace VideoEffects
{
    public sealed class TextVideoCompositor : IVideoCompositor
    {
        private VideoEncodingProperties _backgroundProperties;
        private CanvasDevice _canvasDevice;
        private IPropertySet _configuration;
        private CroppedBounds _croppedBounds;

        public string Text
        {
            get
            {
                return (string)_configuration[nameof(Text)];
            }
        }

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

        public void CompositeFrame(CompositeVideoFrameContext context)
        {
            IDirect3DSurface outputSurface = context.OutputFrame.Direct3DSurface;
            using (CanvasRenderTarget renderTarget = CanvasRenderTarget.CreateFromDirect3D11Surface(_canvasDevice, outputSurface))
            using (CanvasDrawingSession ds = renderTarget.CreateDrawingSession())
            {
                if (string.IsNullOrEmpty(Text))
                    return;

                TimeSpan time = context.OutputFrame.RelativeTime.HasValue ? context.OutputFrame.RelativeTime.Value : new TimeSpan();

                TextFade textFade;
                Color color = Color;

                if (Fade)
                    textFade = new TextFade(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), StartTime);                    
                else
                    textFade = new TextFade(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(0), StartTime);

                var alpha = textFade.GetAplha(time);
                if (alpha == 0) return;
                color.A = alpha;

                if (_croppedBounds == null)
                    _croppedBounds = new CroppedBounds(renderTarget.Bounds);

                ds.DrawText(Text, _croppedBounds.Center, color,
                        new CanvasTextFormat()
                        {
                            FontSize = Convert.ToSingle(0.04 * _croppedBounds.CroppedHeight),
                            FontWeight = new FontWeight() { Weight = 300 },
                            HorizontalAlignment = CanvasHorizontalAlignment.Center,
                            VerticalAlignment = CanvasVerticalAlignment.Center
                        });
            }
        }

        public void SetEncodingProperties(VideoEncodingProperties backgroundProperties, IDirect3DDevice device)
        {
            _backgroundProperties = backgroundProperties;
            _canvasDevice = CanvasDevice.CreateFromDirect3D11Device(device);
            CanvasDevice.DebugLevel = CanvasDebugLevel.Error;
        }

        public void Close(MediaEffectClosedReason reason)
        {
            // Clean up device resources
            if (_canvasDevice != null)
                _canvasDevice.Dispose();
        }

        public void SetProperties(IPropertySet configuration)
        {
            _configuration = configuration;
        }

        public void DiscardQueuedFrames()
        {
            // We don't cache frames, so we have nothing to clean up
        }

        public bool TimeIndependent { get { return true; } }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PropertySet GetDefaultProperties()
        {
            return new PropertySet()
            {
                new KeyValuePair<string, object>(nameof(Text), ""),
                new KeyValuePair<string, object>(nameof(Fade), false),
                new KeyValuePair<string, object>(nameof(Color), Colors.White),
                new KeyValuePair<string, object>(nameof(StartTime), 0.0)
            };
        }
    }
}
