using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Why.Engine;
using Why.Game;
using Why.Game.Components;
using Why.Game.Objects;
using Why.Shared;
using Why.Shared.Components;

namespace Why
{
    public class Why : GameWindow
    {
        private readonly WhyObj _player;
        private readonly List<WhyObj> _tiles;

        public static Why instance = null!;

        public Why(GameWindowSettings windowSettings, NativeWindowSettings nativeWindowSettings) : base(windowSettings, nativeWindowSettings)
        {
            instance = this;
            _player = new WhyObj();
            _player.addComponent(new PlayerComponent());
            _player.addComponent(new FloatPosComponent());
            _player.addComponent(new CubeRenderingComponent());
            _player.getComponent<FloatPosComponent>().pitch = 90;
            _player.getComponent<FloatPosComponent>().yaw = 90;
            _tiles = new List<WhyObj>();
            for (int i = -5; i <= 5; i++)
            {
                for (int j = -5; j <= 5; j++)
                {
                    if (Random.Shared.NextDouble() > 0.5)
                    {
                        var tile = new WhyObj();
                        tile.addComponent(new IntPosComponent(i + 6, 0, j));
                        tile.addComponent(new TileComponent(Sprites.sand));
                        _tiles.Add(tile);
                    }
                    else
                    {
                        var cube = new Cube(CubeTexturing.grass, new(i + 6, 1, j));
                        _tiles.Add(cube);
                    }
                }
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.11f, 0.11f, 0.15f, 1.0f);
            GL.DepthFunc(DepthFunction.Lequal);
            RenderGlobal.updateProjection();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            
            RenderGlobal.updateProjection();
            Fbo.resize(Size.X, Size.Y);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            RenderGlobal.frame.bind();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            RenderGlobal.updateLookAt(_player);
            
            RenderGlobal.tex0.bind(TextureUnit.Texture0);
            RenderGlobal.mesh.begin();
            _player.render();
            foreach (var tile in _tiles)
            {
                tile.render();
            }
            RenderGlobal.mesh.end();
            RenderGlobal.mesh.render();

            RenderGlobal.frame.blit();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            
            _player.update();
        }
    }
}