using OpenTK.Mathematics;
using Why.Game.Components;
using Why.Shared;

namespace Why.Game.Objects
{
    public class Cube : WhyObj
    {

        public readonly CubeTexturing texturing;
        public readonly CubeRenderData data;

        public Cube(CubeTexturing texturing, Vector3i position)
        {
            this.texturing = texturing;
            data = new CubeRenderData();
            addComponent(new IntPosComponent());
            addComponent(new CubeRenderingComponent());
            addComponent(new CollisionComponent(new(position.X * 40, position.Y - 1, position.Z * 40, position.X * 40 + 40, position.Y * 1, position.Z * 40 + 40)));
        }
    }
}