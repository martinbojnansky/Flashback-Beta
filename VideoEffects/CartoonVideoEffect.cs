using Lumia.Imaging;
using Lumia.Imaging.Artistic;
using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation.Collections;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;
using Windows.UI;

namespace VideoEffects
{
    public sealed class CartoonVideoEffect : IBasicVideoEffect
    {
        CanvasDevice canvasDevice;
        Direct3DSurfaceImageSource m_direct3DSurfaceImageSource;
        CartoonEffect m_Effect = new CartoonEffect();
        Direct3DSurfaceRenderer m_direct3DSurfaceRenderer;

        public bool IsReadOnly { get { return true; } }

        public IReadOnlyList<VideoEncodingProperties> SupportedEncodingProperties
        {
            get
            {
                return new List<VideoEncodingProperties>();
            }
        }

        public MediaMemoryTypes SupportedMemoryTypes { get { return MediaMemoryTypes.Gpu; } }

        public bool TimeIndependent { get { return false; } }

        public void Close(MediaEffectClosedReason reason)
        {
            if (canvasDevice != null) { canvasDevice.Dispose(); }
        }

        public void DiscardQueuedFrames() { }

        public void ProcessFrame(ProcessVideoFrameContext context)
        {
            var inputDirect3DSurface = context.InputFrame.Direct3DSurface;

            float anyDpi = 96.0f;
            var width = context.OutputFrame.Direct3DSurface.Description.Width;
            var height = context.OutputFrame.Direct3DSurface.Description.Height;

            CanvasRenderTarget renderTarget = new CanvasRenderTarget(canvasDevice, (float)width, (float)height, anyDpi);

            if (inputDirect3DSurface != null && renderTarget != null)
            {
                if (m_direct3DSurfaceImageSource == null)
                {
                    m_direct3DSurfaceImageSource = new Direct3DSurfaceImageSource();
                }

                m_direct3DSurfaceImageSource.Direct3DSurface = inputDirect3DSurface;
                ((IImageConsumer)m_Effect).Source = m_direct3DSurfaceImageSource;

                if (m_direct3DSurfaceRenderer == null)
                {
                    m_direct3DSurfaceRenderer = new Direct3DSurfaceRenderer();
                }

                m_direct3DSurfaceRenderer.Direct3DSurface = renderTarget;
                m_direct3DSurfaceRenderer.Source = m_Effect;

                var task = m_direct3DSurfaceRenderer.RenderAsync().AsTask();
                task.Wait();

                m_direct3DSurfaceRenderer.Direct3DSurface = null;
                m_direct3DSurfaceImageSource.Direct3DSurface = null;
            }

            // Part 2
            var size = context.OutputFrame.Direct3DSurface.Description.Width * context.OutputFrame.Direct3DSurface.Description.Height;
            var colors = Enumerable.Repeat(Colors.Black, size).ToArray();

            var inputBitmap = CanvasBitmap.CreateFromColors(canvasDevice, colors, width, height, anyDpi);
            inputBitmap.CopyPixelsFromBitmap(renderTarget);

            using (CanvasRenderTarget output = CanvasRenderTarget.CreateFromDirect3D11Surface(canvasDevice, context.OutputFrame.Direct3DSurface))
            using (CanvasDrawingSession ds = output.CreateDrawingSession())
            {
                ds.DrawImage(inputBitmap);
            }

            ((IDisposable)renderTarget).Dispose();
            ((IDisposable)inputBitmap).Dispose();
        }

        public void SetEncodingProperties(VideoEncodingProperties encodingProperties, IDirect3DDevice device)
        {
            canvasDevice = CanvasDevice.CreateFromDirect3D11Device(device);
        }

        public void SetProperties(IPropertySet configuration) { }
    }
}