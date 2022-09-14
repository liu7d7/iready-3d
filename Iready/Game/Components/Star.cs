using Iready.Engine;
using Iready.Shared;
using Iready.Shared.Components;
using Iready.Shared.Tweens;
using OpenTK.Mathematics;

namespace Iready.Game.Components
{
    public class Star : IreadyObj.Component
    {
        private Model3d _model;
        private BaseTween _bob;
        private float _rand;
        
        public Star()
        {
            _model = Model3d.Read("gsprintstar", Maps.Create(new KeyValuePair<string, uint>("Main", 0xffffff00)));
            _bob = new Tween(Animations.UpAndDown(Animations.EaseInOut), 720, true);
            _rand = Random.Shared.NextSingle() * 180;
        }

        public override void Render(IreadyObj objIn)
        {
            base.Render(objIn);
            FloatPos pos = objIn.Get<FloatPos>();
            if (MathF.Abs(Iready.Player.Get<FloatPos>().X - pos.X) > 20)
            {
                return;
            }

            Vector3 renderPos = pos.ToLerpedVec3(0, _bob.Output() * 0.25f - 0.125f + 1.75f, 0);
            RenderSystem.Push();
            RenderSystem.Model.Translate(-renderPos);
            RenderSystem.Model.Rotate((Environment.TickCount + _rand) % 1080 / 3f, Vector3.UnitY);
            RenderSystem.Model.Scale(0.25f);
            RenderSystem.Model.Translate(renderPos);
            _model.Render(renderPos);
            RenderSystem.Pop();
        }

        public override void Collide(IreadyObj objIn, IreadyObj other)
        {
            if (other.Get<Player>() == null) return;
            Iready.Score += 1000;
            objIn.MarkedForRemoval = true;
        }
    }
}