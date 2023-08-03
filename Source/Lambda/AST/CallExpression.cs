using Lambda.Commands;
using Lambda.Environments;
using Lambda.Objects;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Lambda.AST
{
    internal class CallExpression : AbstractExpression
    {
        public AbstractExpression Function { get; private set; }
        public AbstractExpression Argument { get; private set; }

        public CallExpression(AbstractExpression function, AbstractExpression argument)
        {
            this.Function = function;
            this.Argument = argument;
        }

        public override string ToString()
        {
            return "(" + this.Function.ToString() + " " + this.Argument.ToString() + ")";
        }

        public override ICommand Evaluate(IEnvironment environment)
        {
            AggregateCommand buffer = new AggregateCommand();
            buffer.Push(this.Function.Evaluate(environment));
            buffer.Push(new UnaryFunctionCommand((fobj) => fobj.Apply(new Thunk(this.Argument, environment))));

            return buffer;
        }

        public override void Collect<T>(IList<T> nodes)
        {
            T? self = this as T;

            if (self != null)
                nodes.Add(self);

            Function.Collect(nodes);
            Argument.Collect(nodes);
        }
    }
}
