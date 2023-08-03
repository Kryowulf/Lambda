using Lambda.Objects;
using System.Collections.Generic;

namespace Lambda.Commands
{
    internal class EmptyCommand : ICommand
    {
        public static readonly EmptyCommand Instance = new();

        private EmptyCommand() { }

        public void Run(Stack<ICommand> commandStack, Stack<AbstractObject> resultStack)
        {
        }
    }
}
