namespace Why.Shared
{
    public class CubeTexturing
    {
        public static readonly CubeTexturing grass = new(Sprites.rectangle, Sprites.rectangle, Sprites.rectangle, Sprites.rectangle, Sprites.rectangle);
        
        public Sprite top, left, right, front, back;
        
        private CubeTexturing(Sprite top, Sprite left, Sprite right, Sprite front, Sprite back)
        {
            this.top = top;
            this.left = left;
            this.right = right;
            this.front = front;
            this.back = back;
        }
    }
}