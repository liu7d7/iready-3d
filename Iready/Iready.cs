using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Iready.Engine;
using Iready.Game.Components;
using Iready.Shared;
using Iready.Shared.Components;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Iready
{
    public class Iready : GameWindow
    {
        private readonly IreadyObj[] _lane = new IreadyObj[3];
        private readonly List<IreadyObj> _stars = new();
        private readonly List<IreadyObj> _hurdles = new();
        public static IreadyObj Player;
        public static int Ticks;
        public static int Score;

        public static Iready Instance;

        public Iready(GameWindowSettings windowSettings, NativeWindowSettings nativeWindowSettings) : base(windowSettings, nativeWindowSettings)
        {
            Instance = this;
            for (int i = 0; i < 3; i++)
            {
                _lane[i] = new IreadyObj();
                _lane[i].Add(new FloatPos());
                var i1 = i;
                _lane[i].Add(new Model3d.Component(
                    Model3d.Read("lane", Maps.Create<string, uint>(new("Material", 0xff4dabfb), new("Material.001", 0xff4dabfb), new("Material.002", 0xff93daff))),
                    _ =>
                    {
                        RenderSystem.Push();
                        RenderSystem.Model.Scale(1000, 1, 1.5f);
                        RenderSystem.Model.Translate(-5 + i1 * 5 + Player.Get<FloatPos>().X, 0, -5 + i1 * 5);
                    },
                    _ => RenderSystem.Pop()));
            }

            Player = new IreadyObj();
            Player.Add(new FloatPos());
            Player.Add(new Player(Player));
            Player.Add(new Camera());
            Player.Get<FloatPos>().Pitch = 215;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0f, 0f, 0f, 0f);
            GL.DepthFunc(DepthFunction.Lequal);
            GLFW.SwapInterval(0);
            GLFW.WindowHint(WindowHintBool.Resizable, false);
            RenderSystem.UpdateProjection();
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            Ticker.Init();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            RenderSystem.UpdateProjection();
            Fbo.Resize(Size.X, Size.Y);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            RenderSystem.FRAME.Bind();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            RenderSystem.UpdateLookAt(Player, false);
            RenderSystem.STARS.Bind(TextureUnit.Texture0);
            RenderSystem.MESH.Begin();
            RenderStars();
            RenderSystem.MESH.End();
            RenderSystem.MESH.Render();
            Texture.Unbind();

            RenderSystem.UpdateLookAt(Player);
            RenderSystem.TEX0.Bind(TextureUnit.Texture0);
            RenderSystem.MESH.Begin();
            Player.Render();
            for (int i = 0; i < 3; i++)
            {
                _lane[i].Render();
            }

            foreach (IreadyObj star in _stars)
            {
                star.Render();
            }
            
            foreach (IreadyObj hurdle in _hurdles)
            {
                hurdle.Render();
            }

            RenderSystem.MESH.End();
            RenderSystem.MESH.Render();
            Texture.Unbind();
            
            RenderSystem.UpdateLookAt(Player, false);
            RenderSystem.RenderingRed = true;
            RenderSystem.FONT.Bind();
            RenderSystem.MESH.Begin();
            RenderSystem.FONT.Draw(RenderSystem.MESH, Score.ToString(), 3 - Size.X / 2f, -3 + Size.Y / 2f, 0xffffffff, true, 1.5f);
            RenderSystem.MESH.End();
            RenderSystem.MESH.Render();
            Font.Unbind();
            RenderSystem.RenderingRed = false;

            RenderSystem.FRAME.Blit();

            SwapBuffers();
        }

        private void RenderStars()
        {
            FloatPos pos = Player.Get<FloatPos>();
            Mesh mesh = RenderSystem.MESH;
            RenderSystem.Push();
            RenderSystem.Model.Scale(3);
            RenderSystem.Model.Rotate(45, Vector3.UnitZ);
            for (int i = 0; i < 3; i++)
            {
                float x = i * Size.X - Size.X + MathHelper.Lerp(pos.PrevX, pos.X, Ticker.TickDelta) % (Size.X);
                float y = 0;
                mesh.Quad(
                    mesh.Float3(RenderSystem.Model, x - Size.X / 2f, y - Size.Y / 2f, 0).Float3(0, 0, 1).Float2(0, 0).Float4(0xffffffff).Next(),
                    mesh.Float3(RenderSystem.Model, x + Size.X / 2f, y - Size.Y / 2f, 0).Float3(0, 0, 1).Float2(1, 0).Float4(0xffffffff).Next(),
                    mesh.Float3(RenderSystem.Model, x + Size.X / 2f, y + Size.Y / 2f, 0).Float3(0, 0, 1).Float2(1, 1).Float4(0xffffffff).Next(),
                    mesh.Float3(RenderSystem.Model, x - Size.X / 2f, y + Size.Y / 2f, 0).Float3(0, 0, 1).Float2(0, 1).Float4(0xffffffff).Next()
                );
            }
            RenderSystem.Pop();
            RenderSystem.Push();
            RenderSystem.Model.Scale(4.5f);
            RenderSystem.Model.Rotate(45, Vector3.UnitZ);
            for (int i = 0; i < 3; i++)
            {
                float x = i * Size.X - Size.X + MathHelper.Lerp(pos.PrevX, pos.X, Ticker.TickDelta) % (Size.X);
                float y = 0;
                mesh.Quad(
                    mesh.Float3(RenderSystem.Model, x - Size.X / 2f, y - Size.Y / 2f, 0).Float3(0, 0, 1).Float2(0, 0).Float4(0xffffffff).Next(),
                    mesh.Float3(RenderSystem.Model, x + Size.X / 2f, y - Size.Y / 2f, 0).Float3(0, 0, 1).Float2(1, 0).Float4(0xffffffff).Next(),
                    mesh.Float3(RenderSystem.Model, x + Size.X / 2f, y + Size.Y / 2f, 0).Float3(0, 0, 1).Float2(1, 1).Float4(0xffffffff).Next(),
                    mesh.Float3(RenderSystem.Model, x - Size.X / 2f, y + Size.Y / 2f, 0).Float3(0, 0, 1).Float2(0, 1).Float4(0xffffffff).Next()
                );
            }
            RenderSystem.Pop();
            RenderSystem.Push();
            RenderSystem.Model.Scale(6);
            RenderSystem.Model.Rotate(45, Vector3.UnitZ);
            for (int i = 0; i < 3; i++)
            {
                float x = i * Size.X - Size.X + MathHelper.Lerp(pos.PrevX, pos.X, Ticker.TickDelta) % (Size.X);
                float y = 0;
                mesh.Quad(
                    mesh.Float3(RenderSystem.Model, x - Size.X / 2f, y - Size.Y / 2f, 0).Float3(0, 0, 1).Float2(0, 0).Float4(0xffffffff).Next(),
                    mesh.Float3(RenderSystem.Model, x + Size.X / 2f, y - Size.Y / 2f, 0).Float3(0, 0, 1).Float2(1, 0).Float4(0xffffffff).Next(),
                    mesh.Float3(RenderSystem.Model, x + Size.X / 2f, y + Size.Y / 2f, 0).Float3(0, 0, 1).Float2(1, 1).Float4(0xffffffff).Next(),
                    mesh.Float3(RenderSystem.Model, x - Size.X / 2f, y + Size.Y / 2f, 0).Float3(0, 0, 1).Float2(0, 1).Float4(0xffffffff).Next()
                );
            }
            RenderSystem.Pop();
        }
        
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            int i = Ticker.Update();

            Player.Get<FloatPos>().Yaw = -45;

            for (int j = 0; j < Math.Min(10, i); j++)
            {
                Ticks++;

                Player.Update();
                if (Ticks % 5 == 0)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        if (Random.Shared.NextSingle() > 0.9f)
                        {
                            IreadyObj star = new();
                            star.Add(new Tag(k));
                            star.Add(new FloatPos { X = -Ticks - 15, Z = k * 5 - 5, Y = 1 });
                            star.Get<FloatPos>().SetPrev();
                            star.Add(new Star());
                            _stars.Add(star);
                        }
                        else if (Random.Shared.NextSingle() > 0.9f)
                        {
                            IreadyObj hurdle = new();
                            hurdle.Add(new Tag(k));
                            hurdle.Add(new FloatPos { X = -Ticks - 15, Z = k * 5 - 5, Y = 1 });
                            hurdle.Get<FloatPos>().SetPrev();
                            hurdle.Add(new Hurdle());
                            _hurdles.Add(hurdle);
                        }
                    }

                    _stars.RemoveAll(it => it.Get<FloatPos>().X - Player.Get<FloatPos>().X > 18 || it.MarkedForRemoval);
                    _hurdles.RemoveAll(it => it.Get<FloatPos>().X - Player.Get<FloatPos>().X > 18 || it.MarkedForRemoval);
                }
                
                foreach (IreadyObj star in _stars)
                {
                    if (Math.Abs(star.Get<FloatPos>().X - Player.Get<FloatPos>().X - 3) < 2.5 && Player.Get<Player>().Collide && star.Get<Tag>().Id == Player.Get<Player>().Lane)
                    {
                        star.Collide(Player);
                    }
                }
                _stars.RemoveAll(it => it.MarkedForRemoval);
                    
                foreach (IreadyObj hurdle in _hurdles)
                {
                    if (Math.Abs(hurdle.Get<FloatPos>().X - Player.Get<FloatPos>().X - 3) < 2.5 && Player.Get<Player>().Collide && hurdle.Get<Tag>().Id == Player.Get<Player>().Lane)
                    {
                        hurdle.Collide(Player);
                    }
                }
                _hurdles.RemoveAll(it => it.MarkedForRemoval);
            }
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            Player player = Player.Get<Player>();
            switch (e.Key)
            {
                case Keys.Up:
                    player.MoveTo(player.Lane + 1);
                    break;
                case Keys.Down:
                    player.MoveTo(player.Lane - 1);
                    break;
                case Keys.Space:
                    Player.Get<Player>().Jump();
                    break;
            }
        }
    }
}