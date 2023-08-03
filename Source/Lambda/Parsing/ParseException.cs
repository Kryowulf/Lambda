using System;

namespace Lambda.Parsing
{
    internal class ParseException : Exception
    {
        public ParseException() : base() { }
        public ParseException(string message) : base(message) { } 
    }
}
