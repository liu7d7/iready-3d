namespace Iready.Shared
{
    public class IreadyObj
    {
        private readonly HashSet<Component> _components;
        public bool MarkedForRemoval;

        public IreadyObj()
        {
            _components = new HashSet<Component>();
        }

        public void Update()
        {
            foreach (Component component in _components)
            {
                component.Update(this);
            }
        }

        public void Render()
        {
            foreach (Component component in _components)
            {
                component.Render(this);
            }
        }

        public void Collide(IreadyObj other)
        {
            foreach (Component component in _components)
            {
                component.Collide(this, other);
            }
        }

        public void Add(Component component)
        {
            _components.Add(component);
        }
        
        private bool CompFinder<T>(Component comp)
        {
            return typeof(T) == comp.GetType();
        }

        private readonly Dictionary<Type, Component> _cache = new();

        public T Get<T>() where T : Component
        {
            if (_cache.TryGetValue(typeof(T), out Component comp))
            {
                return (T) comp;
            }
            
            T val = (T)_components.FirstOrDefault(CompFinder<T>, null);
            _cache[typeof(T)] = val;
            return val;
        }
        
        public bool Has<T>() where T : Component
        {
            return _components.Any(CompFinder<T>);
        }

        public class Component
        {
            public virtual void Update(IreadyObj objIn)
            {
                
            }

            public virtual void Render(IreadyObj objIn)
            {
                
            }
            
            public virtual void Collide(IreadyObj objIn, IreadyObj other)
            {
                
            }

            public override int GetHashCode()
            {
                return GetType().GetHashCode();
            }
        }
    }
}