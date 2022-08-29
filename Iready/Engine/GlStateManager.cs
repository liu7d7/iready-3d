using OpenTK.Graphics.OpenGL4;

namespace Iready.Engine
{
    public static class GlStateManager
    {
        private static bool _depthEnabled;
        private static bool _blendEnabled;
        private static bool _cullEnabled;

        private static bool _depthSaved;
        private static bool _blendSaved;
        private static bool _cullSaved;

        public static void SaveState()
        {
            _depthSaved = _depthEnabled;
            _blendSaved = _blendEnabled;
            _cullSaved = _cullEnabled;
        }
        
        public static void RestoreState()
        {
            if (_depthSaved)
                EnableDepth();
            else
                DisableDepth();
            if (_blendSaved)
                EnableBlend();
            else
                DisableBlend();
            if (_cullSaved)
                EnableCull();
            else
                DisableCull();
        }
        
        public static void EnableDepth()
        {
            if (!_depthEnabled)
            {
                _depthEnabled = true;
                GL.Enable(EnableCap.DepthTest);
            }
        }
        
        public static void DisableDepth()
        {
            if (_depthEnabled)
            {
                _depthEnabled = false;
                GL.Disable(EnableCap.DepthTest);
            }
        }
        
        public static void EnableBlend()
        {
            if (!_blendEnabled)
            {
                _blendEnabled = true;
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            }
        }
        
        public static void DisableBlend()
        {
            if (_blendEnabled)
            {
                _blendEnabled = false;
                GL.Disable(EnableCap.Blend);
            }
        }
        
        public static void EnableCull()
        {
            if (!_cullEnabled)
            {
                _cullEnabled = true;
                GL.Enable(EnableCap.CullFace);
            }
        }
        
        public static void DisableCull()
        {
            if (_cullEnabled)
            {
                _cullEnabled = false;
                GL.Disable(EnableCap.CullFace);
            }
        }
    }
}