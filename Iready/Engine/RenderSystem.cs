using OpenTK.Mathematics;
using Iready.Shared;
using Iready.Shared.Components;

namespace Iready.Engine
{
    public static class RenderSystem
    {

        private static readonly Shader _JOHN = new("Resource/Shader/john.vert", "Resource/Shader/john.frag");
        private static Matrix4 _projection;
        private static Matrix4 _lookAt;
        private static Matrix4[] _model = new Matrix4[7];
        public static Matrix4 Model;
        private static int _modelIdx;
        public static bool RenderingRed;
        
        static RenderSystem()
        {
            Array.Fill(_model, Matrix4.Identity);
        }

        public static void Push()
        {
            _model[_modelIdx + 1].Set(_model[_modelIdx]);
            _modelIdx++;
            Model.Set(_model[_modelIdx]);
        }

        public static void Pop()
        {
            _modelIdx--;
            Model.Set(_model[_modelIdx]);
        }

        public static readonly Font FONT = new(File.ReadAllBytes("Resource/Font/Dank Mono Regular.otf"), 32);
        public static readonly Texture TEX0 = Texture.LoadFromFile("Resource/Texture/Texture.png");
        public static readonly Texture STARS = Texture.LoadFromFile("Resource/Texture/Stars.png");
        public static readonly Mesh MESH = new(Mesh.DrawMode.TRIANGLE, _JOHN, Vao.Attrib.FLOAT3, Vao.Attrib.FLOAT3, Vao.Attrib.FLOAT2, Vao.Attrib.FLOAT4);
        public static readonly Fbo FRAME = new(Iready.Instance.Size.X, Iready.Instance.Size.Y, true);
        public static bool Rendering3d;
        private static FloatPos _camera;

        public static void SetDefaults(this Shader shader)
        {
            shader.SetMatrix4("_proj", _projection);
            shader.SetMatrix4("_lookAt", _lookAt);
            shader.SetVector2("_screenSize", new(Iready.Instance.Size.X, Iready.Instance.Size.Y));
            shader.SetInt("_rendering3d", Rendering3d ? 1 : 0);
            shader.SetInt("_renderingRed", RenderingRed ? 1 : 0);
            shader.SetVector3("lightPos", new(_camera.X + 5, _camera.Y + 12, _camera.Z + 5));
        }

        public static void UpdateProjection()
        {
            Matrix4.CreateOrthographic(Iready.Instance.Size.X, Iready.Instance.Size.Y, -1000, 3000, out _projection);
        }

        public static void UpdateLookAt(IreadyObj cameraObj, bool rendering3d = true)
        {
            if (!cameraObj.Has<FloatPos>())
            {
                return;
            }

            _camera = cameraObj.Get<FloatPos>();
            Rendering3d = rendering3d;
            if (!Rendering3d)
            {
                _lookAt = Matrix4.Identity;
                return;
            }

            Camera comp = cameraObj.Get<Camera>();
            _lookAt = comp.GetCameraMatrix();
        }
    }
}