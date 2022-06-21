using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Why.Engine;

namespace Why
{
    public class Why : GameWindow
    {
        public static Why instance = null!;
        public Vector2 sizef => new(Size.X, Size.Y);

        public Why(GameWindowSettings windowSettings, NativeWindowSettings nativeWindowSettings) : base(windowSettings, nativeWindowSettings)
        {
            instance = this;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            
            GL.ClearColor(0.11f, 0.11f, 0.15f, 1.0f);
            RenderGlobal.updateProjection();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            RenderGlobal.mesh.begin();
            int i1 = RenderGlobal.mesh.float3(100, 100, 0).float4(0xffffffff).next();
            int i2 = RenderGlobal.mesh.float3(100, 200, 0).float4(0xffffffff).next();
            int i3 = RenderGlobal.mesh.float3(200, 200, 0).float4(0xffffffff).next();
            int i4 = RenderGlobal.mesh.float3(200, 100, 0).float4(0xffffffff).next();
            RenderGlobal.mesh.quad(i1, i2, i3, i4);
            RenderGlobal.mesh.end();
            RenderGlobal.mesh.render();
            
            GL.Flush();
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            
            GL.Viewport(0, 0, Size.X, Size.Y);
            RenderGlobal.updateProjection();
        }
    }
}