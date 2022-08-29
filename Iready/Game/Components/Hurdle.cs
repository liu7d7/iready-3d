using Iready.Shared;
using Iready.Shared.Components;
using OpenTK.Mathematics;

namespace Iready.Game.Components
{
    public class Hurdle : IreadyObj.Component
    {
        private Model3d _model;
        
        public Hurdle()
        {
            _model = Model3d.Read("hurdle", Maps.Create(new KeyValuePair<string, uint>("", 0xffff0192)));
        }

        public override void Render(IreadyObj objIn)
        {
            base.Render(objIn);
            FloatPos pos = objIn.Get<FloatPos>();
            if (MathF.Abs(Iready.Player.Get<FloatPos>().X - pos.X) > 20)
            {
                return;
            }

            Vector3 renderPos = pos.ToLerpedVec3(0, 1, 0);
            _model.Render(renderPos);
        }

        public override void Collide(IreadyObj objIn, IreadyObj other)
        {
            Player player;
            if ((player = other.Get<Player>()) != null)
            {
                player.Speed = 0;
                objIn.MarkedForRemoval = true;
            }
        }
    }
}