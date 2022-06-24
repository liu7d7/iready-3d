namespace Why.Shared
{
    public class CubeTexturing
    {
        public static readonly CubeTexturing grass = new(Sprites.grassTop, Sprites.grassSide, Sprites.grassSide, Sprites.grassSide, Sprites.grassSide);
        public static readonly CubeTexturing player = new(Sprites.playerTop, Sprites.player, Sprites.player, Sprites.player, Sprites.player);
        public static readonly CubeTexturing playerHead = new(Sprites.playerHead, Sprites.playerHead, Sprites.playerHead, Sprites.playerHead, Sprites.playerHead);
        public static readonly CubeTexturing playerWheel = new(Sprites.playerTop, Sprites.playerTop, Sprites.playerTop, Sprites.playerTop, Sprites.playerTop);
        
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