using Lambda.Commands;

namespace Lambda.Objects
{
    internal abstract class AbstractObject
    {
        public abstract ICommand Apply(Thunk argument);
    }
}
