using Lambda.Commands;
using Lambda.Environments;
using Lambda.Objects;
using System.Collections.Generic;

namespace Lambda.AST
{
    internal class LambdaExpression : AbstractExpression
    {
        public string Parameter { get; private set; }
        public AbstractExpression Body { get; private set; }

        public LambdaExpression(string parameter, AbstractExpression body)
        {
            this.Parameter = parameter;
            this.Body = body;
        }

        public override ICommand Evaluate(IEnvironment environment)
        {
            return new ResultCommand(new LambdaObject(environment, this));
        }

        public override string ToString()
        {
            return "," + this.Parameter + " " + this.Body.ToString();
        }

        public override void Collect<T>(IList<T> nodes)
        {
            T? self = this as T;

            if (self != null)
                nodes.Add(self);

            Body.Collect(nodes);
        }
    }
}
