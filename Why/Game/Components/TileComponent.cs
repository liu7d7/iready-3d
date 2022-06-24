using Why.Engine;
using Why.Shared;

namespace Why.Game.Components
{
    public class TileComponent : WhyObj.Component
    {

        private readonly Sprite _sprite;
        private IntPosComponent? _pos;
        private Rect? _bounds;
        
        public TileComponent(Sprite sprite)
        {
            _sprite = sprite;
        }

        public override void render(WhyObj objIn)
        {
            base.render(objIn);

            if (_pos == null)
            {
                _pos = objIn.getComponent<IntPosComponent>();
            }

            if (_bounds == null)
            {
                _bounds = new(_pos.x * 40, _pos.z * 40, 40, 40);
            }
            
            RenderSystem.draw(_bounds, _sprite);
        }
    }
}