using System.Windows.Forms;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX;

using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Factory = SharpDX.Direct2D1.Factory;
using System;
using SharpDX.Mathematics.Interop;

namespace LoveHeart
{
    public class LoveApp : AbstractLove
    {
        public Factory Factory2D { get; private set; }
        public SharpDX.DirectWrite.Factory FactoryDWrite { get; private set; }
        public WindowRenderTarget RenderTarget2D { get; private set; }
        public SolidColorBrush SceneColorBrush { get; private set; }
        LoveDrawer lover;
        protected override void Initialize(Configuration demoConfiguration)
        {
            Factory2D = new SharpDX.Direct2D1.Factory();
            FactoryDWrite = new SharpDX.DirectWrite.Factory();

            HwndRenderTargetProperties properties = new HwndRenderTargetProperties();
            properties.Hwnd = DisplayHandle;
            properties.PixelSize = new SharpDX.Size2(demoConfiguration.Width, demoConfiguration.Height);
            properties.PresentOptions = PresentOptions.None;

            RenderTarget2D = new WindowRenderTarget(Factory2D, new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)), properties);

            RenderTarget2D.AntialiasMode = AntialiasMode.PerPrimitive;


            SceneColorBrush = new SolidColorBrush(RenderTarget2D, Color.Red);

            lover = new LoveDrawer();
            Console.WriteLine("The coordinates are obtained successfully");
            //foreach (var image in drawer.image)
            //{
            //    foreach (var p in image.InsedePoint)
            //        Console.WriteLine(p.x);
            //}
                 
        }
        int frame = 0;
        bool extend = true, shrink = false;
        protected override void Update(MyTime time)
        {
            //Console.WriteLine(time.ElapseTime);
            BeginDraw();
            RenderTarget2D.Clear(null);
            
            foreach(var inp in lover.image[frame].InsedePoint)
            {
                var e = new Ellipse(new SharpDX.Mathematics.Interop.RawVector2((float)inp.x, (float)inp.y), 1, 1);
                RenderTarget2D.FillEllipse(e, new SolidColorBrush(RenderTarget2D, inp.color));
            }
            /*foreach (var oup in lover.image[frame].OutsidePoint)
            {
                var rc = new RawColor4(oup.color.R, oup.color.G, oup.color.B,255);
                var e = new Ellipse(new SharpDX.Mathematics.Interop.RawVector2((float)oup.x, (float)oup.y), 1, 1);
                RenderTarget2D.FillEllipse(e, new SolidColorBrush(RenderTarget2D, rc));
            }*/
            EndDraw();
            if (extend)
            {
                if (frame == 19)
                { shrink = true; extend = false; }
                else ++frame;
            }
            else
            {
                if (frame == 0)
                { shrink = false; extend = true; }
                else --frame;
            }
            base.Update(time);
        }

        protected override void BeginDraw()
        {
            base.BeginDraw();
            RenderTarget2D.BeginDraw();
            
        }

        protected override void EndDraw()
        {
            base.EndDraw();
            RenderTarget2D.EndDraw();
        }
    }
}
