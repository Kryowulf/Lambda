using Lambda.Commands;
using Lambda.Environments;
using System.Collections.Generic;

namespace Lambda.AST
{
    internal class ConditionalExpression : AbstractExpression
    {
        public AbstractExpression Left { get; private set; }
        public AbstractExpression Right { get; private set; }
        public AbstractExpression EqualResult { get; private set; }
        public AbstractExpression DistinctResult { get; private set; }

        public ConditionalExpression(AbstractExpression left, AbstractExpression right, AbstractExpression equalResult, AbstractExpression distinctResult)
        {
            this.Left = left;
            this.Right = right;
            this.EqualResult = equalResult;
            this.DistinctResult = distinctResult;
        }

        public override ICommand Evaluate(IEnvironment environment)
        {
            AggregateCommand buffer = new AggregateCommand();

            buffer.Push(this.Left.Evaluate(environment));
            buffer.Push(this.Right.Evaluate(environment));

            buffer.Push(new BinaryFunctionCommand((x, y) => x.Equals(y) ?
                this.EqualResult.Evaluate(environment) :
                this.DistinctResult.Evaluate(environment)));

            return buffer;
        }

        public override string ToString()
        {
            return this.Left.ToString() + " = " + this.Right.ToString() + " ? " +
                    this.EqualResult.ToString() + " : " + this.DistinctResult.ToString();
        }

        public override void Collect<T>(IList<T> nodes)
        {
            T? self = this as T;

            if (self != null)
                nodes.Add(self);

            Left.Collect(nodes);
            Right.Collect(nodes);
            EqualResult.Collect(nodes);
            DistinctResult.Collect(nodes);
        }
    }
}
