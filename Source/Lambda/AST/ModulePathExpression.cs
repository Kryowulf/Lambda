using Lambda.Commands;
using Lambda.Environments;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lambda.AST
{
    internal class ModulePathExpression : AbstractExpression
    {
        public string ParentFolder { get; private set; }
        public string ModuleFile { get; private set; }
        public string CanonicalPath { get; private set; }
        public string SymbolicName { get; private set; }

        public ModulePathExpression(string modulePath)
        {
            FileInfo moduleFileInfo = new FileInfo(modulePath);

            this.ParentFolder = moduleFileInfo.DirectoryName ?? "";
            this.ModuleFile = moduleFileInfo.Name;
            this.CanonicalPath = moduleFileInfo.FullName.ToLower().Replace("/", "\\");
            this.SymbolicName = "[" + this.CanonicalPath + "]";
        }

        public override ICommand Evaluate(IEnvironment environment)
        {
            return environment[this.SymbolicName].Evaluate();
        }

        public override string ToString()
        {
            return "[" + this.CanonicalPath + "]";
        }

        public override bool Equals(object? obj)
        {
            if (obj is ModulePathExpression mpe)
                return this.SymbolicName.Equals(mpe.SymbolicName);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return this.SymbolicName.GetHashCode();
        }

        public override void Collect<T>(IList<T> nodes)
        {
            T? self = this as T;

            if (self != null)
                nodes.Add(self);
        }
    }
}
