using Lambda.Environments;
using Lambda.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lambda.AST
{
    internal class Module
    {
        public ModulePathExpression Path { get; private set; }
        public IReadOnlyList<ModulePathExpression> Imports { get; private set; }
        public IReadOnlyList<Definition> Definitions { get; private set; }

        public Module(ModulePathExpression path, IReadOnlyList<ModulePathExpression> imports, IReadOnlyList<Definition> definitions)
        {
            this.Path = path;
            this.Imports = imports;
            this.Definitions = definitions;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (ModulePathExpression import in this.Imports)
                sb.AppendLine(":" + import.ToString());

            sb.AppendLine();

            foreach (Definition def in this.Definitions)
            {
                sb.AppendLine(def.ToString());
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public void Collect<T>(IList<T> nodes) where T : AbstractExpression
        {
            foreach (var imp in Imports)
                imp.Collect(nodes);

            foreach (var def in Definitions)
                def.Collect(nodes);
        }
    }
}
