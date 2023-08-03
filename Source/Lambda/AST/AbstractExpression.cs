using Lambda.Commands;
using Lambda.Environments;
using System.Collections.Generic;

namespace Lambda.AST
{
    internal abstract class AbstractExpression
    {
        public abstract ICommand Evaluate(IEnvironment environment);

        public abstract void Collect<T>(IList<T> nodes) where T : AbstractExpression;
    }
}
