using Lambda.AST;
using Lambda.Environments;
using Lambda.Parsing;
using Lambda.Objects;
using System;
using System.IO;

namespace Lambda
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Interpreter inter = new Interpreter();

            if (args.Length == 0)
                inter.RunRepl();
            else
                inter.RunFile(args[0]);
        }
    }
}
