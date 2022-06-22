using OpenTK.Mathematics;
using Why.Game.Components;
using Why.Shared;

namespace Why.Game.Objects
{
    public class World : WhyObj
    {
        public readonly Dictionary<Vector3i, Cube> cubes;

        public World(string id)
        {
            addComponent(new TagComponent(id));
            addComponent(new WorldComponent(this));
            cubes = new Dictionary<Vector3i, Cube>();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        cubes.Add(new(i, j, k), new(CubeTexturing.grass, new(i, j, k)));
                    }
                }
            }
        }
    }
}