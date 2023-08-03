using Lambda.Objects;
using System;
using System.Collections.Generic;

namespace Lambda.Commands
{
    internal class NullaryFunctionCommand : ICommand
    {
        private Func<ICommand> _function;

        public NullaryFunctionCommand(Func<ICommand> function)
        {
            this._function = function;
        }

        public void Run(Stack<ICommand> commandStack, Stack<AbstractObject> resultStack)
        {
            commandStack.Push(_function.Invoke());
        }
    }
}
