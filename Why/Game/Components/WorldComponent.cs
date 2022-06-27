using Why.Game.Objects;
using Why.Shared;

namespace Why.Game.Components
{
    public class WorldComponent : WhyObj.Component
    {

        private readonly World _world;
        public WorldComponent(World world)
        {
            _world = world;
        }

        public override void render(WhyObj obj)
        {
            foreach (var cube in _world.cubes)
            {
                cube.Value.render();
            }
        }

        public override void update(WhyObj obj)
        {
            foreach (var cube in _world.cubes)
            {
                cube.Value.update();
            }
        }
    }
}