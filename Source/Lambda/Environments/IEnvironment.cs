using Lambda.Objects;

namespace Lambda.Environments
{
    internal interface IEnvironment
    {
        public Thunk this[string name] { get; }
    }
}
