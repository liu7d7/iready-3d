using Iready.Engine;
using Iready.Shared;
using Iready.Shared.Components;
using Iready.Shared.Tweens;
using OpenTK.Mathematics;

namespace Iready.Game.Components
{
    public class Player : IreadyObj.Component
    {
        private Model3d _body;
        private Model3d _foot;
        private FloatPos _pos;

        public float XOff;
        public float YOff;
        public float ZOff;
        public int Lane;
        private int _nextLane;
        public new bool Collide;
        public float Speed;

        private BaseTween _jump;
        private BaseTween _feet;
        private BaseTween _feetOverride;
        private BaseTween _move; 

        public Player(IreadyObj objIn)
        {
            Lane = 1;
            ZOff = 0;
            _body = Model3d.Read("gsprintbody", Maps.Create(new KeyValuePair<string, uint>("", 0xffffff00)));
            _foot = Model3d.Read("gsprintfoot", Maps.Create(new KeyValuePair<string, uint>("", 0xffffff00)));
            _feet = new Tween(Animations.UpAndDown(Animations.EaseInOut), 720, true);
            _pos = objIn.Get<FloatPos>();
        }

        public override void Render(IreadyObj objIn)
        {
            base.Render(objIn);
            if (_jump?.Done() == true)
            {
                _jump = null;
                _feetOverride = null;
            }

            if (_move?.Done() == true)
            {
                _move = null;
                Lane = _nextLane;
            }
            else if (_move != null)
            {
                ZOff = _move.Output();
            }
            float animation = _feetOverride?.Output() ?? _feet.Output();
            RenderSystem.Push();
            if (_jump != null)
            {
                YOff = MathHelper.Clamp(_jump.Output() * 10 - 5f, 0, 5f);
                RenderSystem.Model.Translate(-_pos.ToLerpedVec3(XOff + 1, YOff, ZOff));
                RenderSystem.Model.Rotate(_jump.Output() * 60 - 30, Vector3.UnitZ);
                RenderSystem.Model.Translate(_pos.ToLerpedVec3(XOff + 1, YOff, ZOff));
            }
            else
            {
                YOff = animation * 0.125f;
            }
            _body.Render(_pos.ToLerpedVec3(XOff, YOff + 1, ZOff));
            if (_jump == null)
            {
                YOff = 0;
            }
            RenderSystem.Pop();

            YOff *= 1.1f;
            
            {
                RenderSystem.Push();
                float angle = animation * 90 - 30;
                RenderSystem.Model.Translate(-_pos.ToLerpedVec3(XOff, YOff, ZOff) - new Vector3(1, 3, 0));
                RenderSystem.Model.Rotate(angle, Vector3.UnitZ);
                RenderSystem.Model.Translate(_pos.ToLerpedVec3(XOff, YOff, ZOff) + new Vector3(1, 3, 0));
                _foot.Render(_pos.ToLerpedVec3(XOff, YOff, ZOff) + new Vector3(1, 0.25f, 0.5f));
                RenderSystem.Pop();
            }

            {
                RenderSystem.Push();
                float angle = (1 - animation) * 90 - 30;
                RenderSystem.Model.Translate(-_pos.ToLerpedVec3(XOff, YOff, ZOff) - new Vector3(1, 3, 0));
                RenderSystem.Model.Rotate(angle, Vector3.UnitZ);
                RenderSystem.Model.Translate(_pos.ToLerpedVec3(XOff, YOff, ZOff) + new Vector3(1, 3, 0));
                _foot.Render(_pos.ToLerpedVec3(XOff, YOff, ZOff) + new Vector3(1, 0.25f, -0.5f));
                RenderSystem.Pop();
            }
        }

        public void Jump()
        {
            if (_jump == null)
            {
                _jump = new ListTween(new FromToTween(Animations.Decelerate, 0.5f, 1, 400), new FromToTween(Animations.EaseInOut, 1, 0.5f, 400));
                bool toZero = !((Environment.TickCount + 800 - _feet.LastActivation) % 720 > 360);
                _feetOverride = new ListTween(
                    new FromToTween(Animations.EaseInOut, _feet.Output(), toZero ? 0 : 1, 300), new StaticTween(toZero ? 0 : 1, 100),
                    new FromToTween(Animations.EaseInOut, toZero ? 0 : 1, _feet.OutputAt(Environment.TickCount + 800), 400));
            }
        }

        public void MoveTo(int lane)
        {
            if (lane == Lane)
            {
                return;
            }
            
            if (_move == null)
            {
                _nextLane = MathHelper.Clamp(lane, 0, 2);
                _move = new FromToTween(Animations.EaseInOut, ZOff, -5f + _nextLane * 5f, 100f);
            }
        }

        public override void Update(IreadyObj objIn)
        {
            Collide = _jump == null || Environment.TickCount - _jump.LastActivation is < 100 or > 700;
            if (_move != null)
            {
                Collide = false;
            }
            objIn.Get<FloatPos>().SetPrev();
            objIn.Get<FloatPos>().X -= Speed;
            Speed = MathHelper.Lerp(Speed, 1.0f, 0.05f);
        }
    }
}