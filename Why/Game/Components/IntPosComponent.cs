namespace Why.Game.Components
{
    public class IntPosComponent : WhyObj.Component
    {
        public int x;
        public int y;
        public int z;

        public IntPosComponent()
        {
            
        }
        
        public IntPosComponent(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}