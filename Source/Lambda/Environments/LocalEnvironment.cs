using Lambda.Objects;

namespace Lambda.Environments
{
    internal class LocalEnvironment : IEnvironment
    {
        private IEnvironment _parent;
        private string _name;
        private Thunk _value;

        public Thunk this[string name]
        {
            get
            {
                if (_name == name)
                    return _value;
                else
                    return _parent[name];
            }
        }

        public LocalEnvironment(IEnvironment parent, string name, Thunk value)
        {
            _parent = parent;
            _name = name;
            _value = value;
        }
    }
}
