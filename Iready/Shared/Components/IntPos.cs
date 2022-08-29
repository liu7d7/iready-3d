using OpenTK.Mathematics;

namespace Iready.Shared.Components
{
    public class IntPos : IreadyObj.Component
    {
        public int X;
        public int Y;
        public int Z;

        public IntPos()
        {
            
        }
        
        public IntPos(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public IntPos(Vector3i pos)
        {
            X = pos.X;
            Y = pos.Y;
            Z = pos.Z;
        }
    }
}