using Lambda.Commands;
using System;

namespace Lambda.Objects
{
    internal class InputObject : AbstractObject
    {
        private Thunk? _head;
        private Thunk? _tail;

        public override ICommand Apply(Thunk argument)
        {
            if (_head == null)
            {
                int codepoint = Console.Read();

                SymbolObject symbol = codepoint >= 0 ? new SymbolObject(char.ConvertFromUtf32(codepoint)) 
                                                     : SymbolObject.Empty;

                _head = new Thunk(symbol);
                _tail = new Thunk(new InputObject());
            }

            AggregateCommand buffer = new AggregateCommand();

            buffer.Push(argument.Evaluate());
            buffer.Push(new UnaryFunctionCommand((fobj) => fobj.Apply(_head!)));
            buffer.Push(new UnaryFunctionCommand((fobj) => fobj.Apply(_tail!)));

            return buffer;
        }

        public override string ToString()
        {
            return "[<STDIN>]";
        }
    }
}
