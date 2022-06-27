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
        private readonly Dictionary<Vector3i, WhyObj> _tiles;
        private readonly IntPosComponent _playerPosi;

        public static Why instance = null!;

        public Why(GameWindowSettings windowSettings, NativeWindowSettings nativeWindowSettings) : base(windowSettings, nativeWindowSettings)
        {
            instance = this;
            _player = new WhyObj();
            _player.addComponent(new PlayerComponent());
            _player.addComponent(new FloatPosComponent());
            _player.addComponent(new IntPosComponent());
            _playerPosi = _player.getComponent<IntPosComponent>();
            _player.addComponent(new CameraComponent());
            _player.getComponent<FloatPosComponent>().yaw = 45;
            _player.getComponent<FloatPosComponent>().pitch = 215;
            _tiles = new Dictionary<Vector3i, WhyObj>();
            for (int i = -125; i <= 125; i++)
            {
                for (int j = -125; j <= 125; j++)
                {
                    if (Random.Shared.NextDouble() > 0.5)
                    {
                        WhyObj tile = new();
                        tile.addComponent(new IntPosComponent(i + 6, 0, j));
                        tile.addComponent(new TileComponent(Sprites.sand));
                        _tiles.Add(new(i + 6, 0, j), tile);
                    }
                    else
                    {
                        Cube cube = new(CubeTexturing.grass, new(i + 6, 1, j));
                        _tiles.Add(new(i + 6, 1, j), cube);
                        updateCRDs(cube);
                    }
                }
            }
            for (int i = -125; i <= 125; i++)
            {
                for (int j = -125; j <= 125; j++)
                {
                    if (Random.Shared.NextDouble() > 0.75)
                    {
                        Cube cube = new(CubeTexturing.grass, new(i + 6, 2, j));
                        _tiles.Add(new(i + 6, 2, j), cube);
                        updateCRDs(cube);
                    }
                }
            }
            for (int i = -125; i <= 125; i++)
            {
                for (int j = -125; j <= 125; j++)
                {
                    if (Random.Shared.NextDouble() > 0.875)
                    {
                        Cube cube = new(CubeTexturing.grass, new(i + 6, 3, j));
                        _tiles.Add(new(i + 6, 3, j), cube);
                        updateCRDs(cube);
                    }
                }
            }
            for (int i = -125; i <= 125; i++)
            {
                for (int j = -125; j <= 125; j++)
                {
                    if (Random.Shared.NextDouble() > 0.975)
                    {
                        Cube cube = new(CubeTexturing.grass, new(i + 6, 4, j));
                        _tiles.Add(new(i + 6, 4, j), cube);
                        updateCRDs(cube);
                    }
                }
            }
        }

        private void updateCRDs(Cube cubeIn)
        {
            int i = cubeIn.getComponent<IntPosComponent>().x;
            int j = cubeIn.getComponent<IntPosComponent>().z;
            int k = cubeIn.getComponent<IntPosComponent>().y;
            Vector3i left = new(i - 1, k, j);
            Vector3i front = new(i, k, j - 1);
            Vector3i down = new(i, k - 1, j);
            if (_tiles.ContainsKey(left))
            {
                if (_tiles[left] is Cube cube)
                {
                    cube.data.drawRight = false;
                    cubeIn.data.drawLeft = false;
                }
            }

            if (_tiles.ContainsKey(front))
            {
                if (_tiles[front] is Cube cube)
                {
                    cube.data.drawBack = false;
                    cubeIn.data.drawFront = false;
                }
            }

            if (_tiles.ContainsKey(down))
            {
                if (_tiles[down] is Cube cube)
                {
                    cube.data.drawTop = false;
                }
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0f, 0f, 0f, 0f);
            GL.DepthFunc(DepthFunction.Lequal);
            RenderSystem.updateProjection();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            
            RenderSystem.updateProjection();
            Fbo.resize(Size.X, Size.Y);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            
            RenderSystem.frame.bind();
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            RenderSystem.updateLookAt(_player);
            
            RenderSystem.tex0.bind(TextureUnit.Texture0);
            RenderSystem.mesh.begin();
            _player.render();
            foreach (var tile in _tiles)
            {
                if (new Vector2i(tile.Key.X, tile.Key.Z).distanceSq(new(_playerPosi.x, _playerPosi.z)) > 484)
                {
                    continue;
                }
                tile.Value.render();
            }
            RenderSystem.mesh.end();
            RenderSystem.mesh.render();

            RenderSystem.frame.blit();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            
            _player.update();
        }
    }
}