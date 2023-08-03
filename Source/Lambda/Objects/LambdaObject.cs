using Lambda.AST;
using Lambda.Commands;
using Lambda.Environments;

namespace Lambda.Objects
{
    internal class LambdaObject : AbstractObject
    {
        public IEnvironment Closure { get; private set; }
        public LambdaExpression Expression { get; private set; }

        public LambdaObject(IEnvironment closure, LambdaExpression expression)
        {
            this.Closure = closure;
            this.Expression = expression;
        }

        public override ICommand Apply(Thunk argument)
        {
            LocalEnvironment localEnv = new LocalEnvironment(
                this.Closure, this.Expression.Parameter, argument);

            return this.Expression.Body.Evaluate(localEnv);
        }

        public override string ToString()
        {
            return "[<LAMBDA>]";
        }
    }
}
