using System.Collections.Generic;

namespace Lambda.AST
{
    internal class Definition
    {
        public string Name { get; private set; }
        public AbstractExpression Value { get; private set; }

        public Definition(string name, AbstractExpression value)
        {
            this.Name = name;
            this.Value = value;
        }

        public override string ToString()
        {
            return this.Name + " " + this.Value.ToString() + ";";
        }

        public void Collect<T>(IList<T> nodes) where T : AbstractExpression
        {
            Value.Collect(nodes);
        }
    }
}
