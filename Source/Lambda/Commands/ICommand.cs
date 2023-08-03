using Lambda.Objects;
using System.Collections.Generic;

namespace Lambda.Commands
{
    internal interface ICommand
    {
        public void Run(Stack<ICommand> commandStack, Stack<AbstractObject> resultStack);
    }
}
