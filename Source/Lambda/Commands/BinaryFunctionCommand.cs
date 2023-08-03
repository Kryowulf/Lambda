using Lambda.Objects;
using System;
using System.Collections.Generic;

namespace Lambda.Commands
{
    internal class BinaryFunctionCommand : ICommand
    {
        private Func<AbstractObject, AbstractObject, ICommand> _function;

        public BinaryFunctionCommand(Func<AbstractObject, AbstractObject, ICommand> function)
        {
            this._function = function;
        }

        public void Run(Stack<ICommand> commandStack, Stack<AbstractObject> resultStack)
        {
            AbstractObject obj2 = resultStack.Pop();
            AbstractObject obj1 = resultStack.Pop();
            commandStack.Push(_function.Invoke(obj1, obj2));
        }
    }
}
