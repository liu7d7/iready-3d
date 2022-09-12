using Iready.Engine;
using Iready.Shared;
using Iready.Shared.Components;
using OpenTK.Mathematics;

namespace Iready.Game.Components
{
    public class Model3d
    {
        private static readonly Dictionary<string, Model3d> _COMPONENTS = new();

        private readonly List<VertexData[]> _faces = new();
        private readonly List<VertexData[]> _lines = new();

        public class Component : IreadyObj.Component
        {
            private readonly Model3d _model;
            private readonly Func _before = Empty;
            private readonly Func _after = Empty;

            public delegate void Func(IreadyObj objIn);

            private static void Empty(IreadyObj obj)
            {
            }

            public Component(Model3d model, Func before, Func after)
            {
                _model = model;
                _before = before;
                _after = after;
            }
            
            public Component(Model3d model)
            {
                _model = model;
            }

            public override void Render(IreadyObj objIn)
            {
                base.Render(objIn);

                _before(objIn);
                _model.Render(objIn.Get<FloatPos>().ToVec3());
                _after(objIn);
            }
        }

        private Model3d(string path, Dictionary<string, uint> colors)
        {
            List<Vector3> vertices = new();
            List<Vector3> normals = new();
            List<Vector2> texCoords = new();
            string mat = "";
            foreach (string line in File.ReadAllLines($"Resource/Texture/{path}.obj"))
            {
                if (line.StartsWith("#"))
                {
                    continue;
                }

                string[] parts = line.Split(' ');
                switch (parts[0])
                {
                    case "v":
                    {
                        Vector3 vec = new();
                        vec.X = float.Parse(parts[1]);
                        vec.Y = float.Parse(parts[2]);
                        vec.Z = float.Parse(parts[3]);
                        vertices.Add(vec);
                        break;
                    }
                    
                    case "vn":
                    {
                        Vector3 vec = new();
                        vec.X = float.Parse(parts[1]);
                        vec.Y = float.Parse(parts[2]);
                        vec.Z = float.Parse(parts[3]);
                        vec.Normalize();
                        normals.Add(vec);
                        break;
                    }
                    case "vt":
                    {
                        Vector2 vec = new();
                        vec.X = float.Parse(parts[1]);
                        vec.Y = float.Parse(parts[2]);
                        texCoords.Add(vec);
                        break;
                    }
                    case "usemtl":
                        mat = parts[1];
                        break;
                    case "f":
                    {
                        string[] vt1 = parts[1].Split("/");
                        string[] vt2 = parts[2].Split("/");
                        string[] vt3 = parts[3].Split("/");
                        VertexData[] face = new VertexData[3];
                        face[0] = new VertexData(vertices[int.Parse(vt1[0]) - 1], texCoords[int.Parse(vt1[1]) - 1]);
                        face[1] = new VertexData(vertices[int.Parse(vt2[0]) - 1], texCoords[int.Parse(vt2[1]) - 1]);
                        face[2] = new VertexData(vertices[int.Parse(vt3[0]) - 1], texCoords[int.Parse(vt3[1]) - 1]);
                        uint val;
                        val = colors.TryGetValue(mat, out val) ? val : 0xFFFFFFFF;
                        face[0].Color = face[1].Color = face[2].Color = val;
                        face[0].Normal = normals[int.Parse(vt1[2]) - 1];
                        face[1].Normal = normals[int.Parse(vt2[2]) - 1];
                        face[2].Normal = normals[int.Parse(vt3[2]) - 1];
                        _faces.Add(face);
                        break;
                    }
                }
            }
            _COMPONENTS[path + colors.ContentToString()] = this;
        }

        public void Render(Vector3 pos)
        {
            Mesh mesh = RenderSystem.MESH;
            foreach (VertexData[] face in _faces)
            {
                Vector3 vt1 = face[0].Pos;
                Vector3 vt2 = face[1].Pos;
                Vector3 vt3 = face[2].Pos;
                int i1 = mesh.Float3(RenderSystem.Model, vt1.X + pos.X, vt1.Y + pos.Y, vt1.Z + pos.Z).Float3(face[0].Normal).Float2(face[0].Uv).Float4(face[0].Color).Next();
                int i2 = mesh.Float3(RenderSystem.Model, vt2.X + pos.X, vt2.Y + pos.Y, vt2.Z + pos.Z).Float3(face[1].Normal).Float2(face[1].Uv).Float4(face[1].Color).Next();
                int i3 = mesh.Float3(RenderSystem.Model, vt3.X + pos.X, vt3.Y + pos.Y, vt3.Z + pos.Z).Float3(face[2].Normal).Float2(face[2].Uv).Float4(face[2].Color).Next();
                mesh.Tri(i1, i2, i3);
            }
        }

        public static Model3d Read(string path, Dictionary<string, uint> colors)
        {
            if (_COMPONENTS.ContainsKey(path + colors.ContentToString()))
            {
                return _COMPONENTS[path + colors.ContentToString()];
            }
            
            return new(path, colors);
        }
    }
}