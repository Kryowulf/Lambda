using System.Collections.Generic;

namespace Lambda.AST
{
    internal class DefinitionShellStatement : ShellStatement
    {
        public Definition Definition { get; private set; }

        public DefinitionShellStatement(Definition definition)
        {
            this.Definition = definition;
        }

        public override void Collect<T>(IList<T> nodes)
        {
            this.Definition.Collect(nodes);
        }
    }
}
