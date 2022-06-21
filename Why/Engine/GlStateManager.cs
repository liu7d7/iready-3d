using OpenTK.Graphics.OpenGL4;

namespace Why.Engine
{
    public static class GlStateManager
    {
        private static bool _depthEnabled;
        private static bool _blendEnabled;
        private static bool _cullEnabled;

        private static bool _depthSaved;
        private static bool _blendSaved;
        private static bool _cullSaved;

        public static void saveState()
        {
            _depthSaved = _depthEnabled;
            _blendSaved = _blendEnabled;
            _cullSaved = _cullEnabled;
        }
        
        public static void restoreState()
        {
            if (_depthSaved)
                enableDepth();
            else
                disableDepth();
            if (_blendSaved)
                enableBlend();
            else
                disableBlend();
            if (_cullSaved)
                enableCull();
            else
                disableCull();
        }
        
        public static void enableDepth()
        {
            if (!_depthEnabled)
            {
                _depthEnabled = true;
                GL.Enable(EnableCap.DepthTest);
            }
        }
        
        public static void disableDepth()
        {
            if (_depthEnabled)
            {
                _depthEnabled = false;
                GL.Disable(EnableCap.DepthTest);
            }
        }
        
        public static void enableBlend()
        {
            if (!_blendEnabled)
            {
                _blendEnabled = true;
                GL.Enable(EnableCap.Blend);
            }
        }
        
        public static void disableBlend()
        {
            if (_blendEnabled)
            {
                _blendEnabled = false;
                GL.Disable(EnableCap.Blend);
            }
        }
        
        public static void enableCull()
        {
            if (!_cullEnabled)
            {
                _cullEnabled = true;
                GL.Enable(EnableCap.CullFace);
            }
        }
        
        public static void disableCull()
        {
            if (_cullEnabled)
            {
                _cullEnabled = false;
                GL.Disable(EnableCap.CullFace);
            }
        }
    }
}