using Lambda.Objects;
using System;
using System.Collections.Generic;

namespace Lambda.Commands
{
    internal class UnaryFunctionCommand : ICommand
    {
        private Func<AbstractObject, ICommand> _function;

        public UnaryFunctionCommand(Func<AbstractObject, ICommand> function)
        {
            this._function = function;
        }

        public void Run(Stack<ICommand> commandStack, Stack<AbstractObject> resultStack)
        {
            AbstractObject obj1 = resultStack.Pop();
            commandStack.Push(_function.Invoke(obj1));
        }
    }
}
