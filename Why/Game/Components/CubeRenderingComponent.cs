using Why.Engine;
using Why.Game.Objects;

namespace Why.Game.Components
{
    public class CubeRenderingComponent : WhyObj.Component
    {
        public CubeRenderingComponent()
        {
            
        }

        public override void render(WhyObj objIn)
        {
            if (objIn is Cube c)
            {
                base.render(objIn);
                
                var aabb = objIn.getComponent<CollisionComponent>().bounds;
                RenderGlobal.draw(aabb, c.texturing, c.data);
            }
        }
    }
}