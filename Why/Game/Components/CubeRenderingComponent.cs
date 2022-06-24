using Why.Engine;
using Why.Game.Objects;
using Why.Shared;

namespace Why.Game.Components
{
    public class CubeRenderingComponent : WhyObj.Component
    {
        private AABB? _aabb;
        
        public CubeRenderingComponent()
        {
            
        }

        public override void render(WhyObj objIn)
        {
            if (objIn is Cube c)
            {
                base.render(objIn);
                
                if (_aabb == null)
                {
                    _aabb = objIn.getComponent<CollisionComponent>().bounds;
                }
                
                RenderSystem.draw(_aabb, c.texturing, c.data);
            }
        }
    }
}