using Lambda.Objects;
using System.Collections.Generic;

namespace Lambda.Commands
{
    internal class ResultCommand : ICommand
    {
        public AbstractObject Result { get; private set; }

        public ResultCommand(AbstractObject result)
        {
            this.Result = result;
        }

        public void Run(Stack<ICommand> commandStack, Stack<AbstractObject> resultStack)
        {
            resultStack.Push(this.Result);
        }
    }
}
