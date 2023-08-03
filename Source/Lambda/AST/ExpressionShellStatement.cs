using System.Collections.Generic;

namespace Lambda.AST
{
    internal class ExpressionShellStatement : ShellStatement
    {
        public AbstractExpression Expression { get; private set; }

        public ExpressionShellStatement(AbstractExpression expression)
        {
            this.Expression = expression;
        }

        public override void Collect<T>(IList<T> nodes)
        {
            this.Expression.Collect(nodes);
        }
    }
}
