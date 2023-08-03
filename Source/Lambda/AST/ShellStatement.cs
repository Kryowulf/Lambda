using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lambda.AST
{
    internal abstract class ShellStatement
    {
        public abstract void Collect<T>(IList<T> nodes) where T : AbstractExpression;
    }
}
