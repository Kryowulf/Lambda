using Lambda.Objects;
using System;
using System.Collections.Generic;

namespace Lambda.Commands
{
    internal class AggregateCommand : ICommand
    {
        private List<ICommand> _subcommands = new();
        private bool _executed = false;

        public void Push(ICommand command)
        {
            if (_executed)
                throw new InvalidOperationException("Attempted to push subcommands after execution.");

            _subcommands.Add(command);
        }

        public void Run(Stack<ICommand> commandStack, Stack<AbstractObject> resultStack)
        {
            _executed = true;
            _subcommands.Reverse();

            foreach (ICommand cmd in _subcommands)
                commandStack.Push(cmd);
        }
    }
}
