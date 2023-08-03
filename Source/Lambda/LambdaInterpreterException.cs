using System;

namespace Lambda
{
    internal class LambdaInterpreterException : Exception
    {
        public LambdaInterpreterException() : base()
        {
        }

        public LambdaInterpreterException(string message) : base(message)
        {
        }
    }
}
