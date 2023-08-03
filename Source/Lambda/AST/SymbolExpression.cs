using Lambda.Commands;
using Lambda.Environments;
using Lambda.Objects;
using System.Collections.Generic;

namespace Lambda.AST
{
    internal class SymbolExpression : AbstractExpression
    {
        public string Name { get; private set; }
        public bool IsQuoted { get; private set; }

        public SymbolExpression(string name, bool isQuoted)
        {
            this.Name = name;
            this.IsQuoted = isQuoted;
        }

        public override ICommand Evaluate(IEnvironment environment)
        {
            if (this.IsQuoted)
                return new ResultCommand(new SymbolObject(this.Name));
            else
                return new NullaryFunctionCommand(() => environment[this.Name].Evaluate());
        }

        public override string ToString()
        {
            return this.IsQuoted ? "'" + this.Name + "'" : this.Name;
        }

        public override void Collect<T>(IList<T> nodes)
        {
            T? self = this as T;

            if (self != null)
                nodes.Add(self);
        }
    }
}
