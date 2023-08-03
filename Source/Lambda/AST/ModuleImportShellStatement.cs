using System.Collections.Generic;

namespace Lambda.AST
{
    internal class ModuleImportShellStatement : ShellStatement
    {
        public ModulePathExpression ModulePath { get; private set; }

        public ModuleImportShellStatement(ModulePathExpression modulepath)
        {
            this.ModulePath = modulepath;
        }

        public override void Collect<T>(IList<T> nodes)
        {
            this.ModulePath.Collect(nodes);
        }
    }
}
