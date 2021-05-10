using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using Microsoft.Graphics.Canvas.Text;
using Windows.UI.Text;

namespace VideoEffects
{
    public sealed class WatermarkVideoCompositor : IVideoCompositor
    {
        private VideoEncodingProperties _backgroundProperties;
        private CanvasDevice _canvasDevice;
        private IPropertySet _configuration;
        private CroppedBounds _croppedBounds;
     
        public void CompositeFrame(CompositeVideoFrameContext context)
        {
            IDirect3DSurface outputSurface = context.OutputFrame.Direct3DSurface;
            using (CanvasRenderTarget renderTarget = CanvasRenderTarget.CreateFromDirect3D11Surface(_canvasDevice, outputSurface))
            using (CanvasDrawingSession ds = renderTarget.CreateDrawingSession())
            {
                if (_croppedBounds == null)
                    _croppedBounds = new CroppedBounds(renderTarget.Bounds);

                ds.DrawText(" Ö Flashback", _croppedBounds.Center, Windows.UI.Colors.White,
                        new CanvasTextFormat()
                        {
                            FontFamily = "ms-appx:///Assets/Fonts/Flashback.ttf#Signika",
                            FontSize = Convert.ToSingle(0.06 * _croppedBounds.CroppedHeight),
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

        public bool TimeIndependent { get { return false; } }
    }
}
