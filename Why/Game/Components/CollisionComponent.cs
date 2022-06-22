using Why.Shared;

namespace Why.Game.Components
{
    public class CollisionComponent : WhyObj.Component
    {
        public AABB bounds;
        
        public CollisionComponent()
        {
            
        }

        public CollisionComponent(AABB bounds)
        {
            this.bounds = bounds;
        }
    }
}