using Lambda.AST;
using Lambda.Commands;
using Lambda.Environments;

namespace Lambda.Objects
{
    internal class ModuleInstance : AbstractObject
    {
        public ModuleEnvironment Environment { get; private set; }
        public Module Module { get; private set; }

        public ModuleInstance(ModuleEnvironment environment, Module module)
        {
            this.Environment = environment;
            this.Module = module;
        }

        public override ICommand Apply(Thunk argument)
        {
            AggregateCommand buffer = new AggregateCommand();

            buffer.Push(argument.Evaluate());

            buffer.Push(new UnaryFunctionCommand((obj) =>
            {
                if (obj is SymbolObject symobj)
                {
                    return this.Environment[symobj.Name].Evaluate();
                }
                else
                {
                    return new ResultCommand(SymbolObject.Empty);
                }

            }));

            return buffer;
        }

        public override string ToString()
        {
            return "[<MODULE>]";
        }
    }
}
