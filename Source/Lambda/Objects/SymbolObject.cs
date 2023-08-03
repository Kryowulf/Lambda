using Lambda.Commands;

namespace Lambda.Objects
{
    internal class SymbolObject : AbstractObject
    {
        public static readonly SymbolObject Empty = new("");

        public string Name { get; private set; }

        public SymbolObject(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public override bool Equals(object? obj)
        {
            if (obj is SymbolObject other)
                return this.Name == other.Name;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override ICommand Apply(Thunk argument)
        {
            return new ResultCommand(Empty);
        }
    }
}
