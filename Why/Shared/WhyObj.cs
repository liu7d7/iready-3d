namespace Why.Shared
{
    public class WhyObj
    {
        private readonly HashSet<Component> _components;

        public WhyObj()
        {
            _components = new HashSet<Component>();
        }

        public void update()
        {
            foreach (Component component in _components)
            {
                component.update(this);
            }
        }

        public void render()
        {
            foreach (Component component in _components)
            {
                component.render(this);
            }
        }

        public void addComponent(Component component)
        {
            _components.Add(component);
        }
        
        private bool compFinder<T>(Component comp)
        {
            return typeof(T) == comp.GetType();
        }

        public T getComponent<T>() where T : Component
        {
            return (T) _components.First(compFinder<T>);
        }
        
        public bool hasComponent<T>() where T : Component
        {
            return _components.Any(compFinder<T>);
        }

        public class Component
        {
            public virtual void update(WhyObj objIn)
            {
                
            }

            public virtual void render(WhyObj objIn)
            {
                
            }

            public override int GetHashCode()
            {
                return GetType().GetHashCode();
            }
        }
    }
}