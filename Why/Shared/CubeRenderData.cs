namespace Why.Shared
{
    public struct CubeRenderData
    {
        public bool drawTop, drawLeft, drawRight, drawFront, drawBack;

        public CubeRenderData()
        {
            drawTop = drawLeft = drawRight = drawFront = drawBack = true;
        }
    }
}