namespace Iready.Shared.Components
{
    public class Tag : IreadyObj.Component
    {
        public int Id;
        public string Name;

        public Tag(int id, string name = "")
        {
            Id = id;
            Name = name;
        }
    }
}