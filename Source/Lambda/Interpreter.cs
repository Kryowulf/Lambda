using Lambda.AST;
using Lambda.Commands;
using Lambda.Environments;
using Lambda.Objects;
using Lambda.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using static System.Reflection.CustomAttributeExtensions;

namespace Lambda
{
    internal class Interpreter
    {
        public void RunRepl()
        {
            Console.WriteLine("Lambda Shell " + getVersion());

            GlobalEnvironment genv = new GlobalEnvironment();
            ModuleEnvironment menv = new ModuleEnvironment(genv);

            menv["_STDIN_"] = new Thunk(new InputObject());

            Parser parser = new Parser(Environment.CurrentDirectory, new Scanner());

            while (true)
            {
                try
                {
                    Console.Write(">");
                    ShellStatement statement = parser.ParseShellStatement();
                    List<ModulePathExpression> paths = new List<ModulePathExpression>();
                    statement.Collect(paths);

                    foreach (ModulePathExpression path in paths)
                        LoadModules(path, genv);

                    if (statement is ExpressionShellStatement)
                    {
                        ExpressionShellStatement expressionStatement = (ExpressionShellStatement)statement;
                        Display(Resolve(expressionStatement.Expression.Evaluate(menv)));
                    }
                    else if (statement is DefinitionShellStatement)
                    {
                        DefinitionShellStatement definitionStatement = (DefinitionShellStatement)statement;
                        menv[definitionStatement.Definition.Name] = new Thunk(definitionStatement.Definition.Value, menv);
                    }
                    else if (statement is ModuleImportShellStatement)
                    {
                        ModuleImportShellStatement importStatement = (ModuleImportShellStatement)statement;
                        AggregateCommand importCommand = new AggregateCommand();

                        importCommand.Push(importStatement.ModulePath.Evaluate(menv));

                        importCommand.Push(new UnaryFunctionCommand((obj) =>
                        {
                            if (obj is ModuleInstance mobj)
                                menv.Import(mobj.Environment);

                            return EmptyCommand.Instance;
                        }));

                        importCommand.Push(new ResultCommand(SymbolObject.Empty));

                        Resolve(importCommand);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    parser.SetSource(Environment.CurrentDirectory, new Scanner());
                }
            }
        }

        public string getVersion()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var attribute = assembly.GetCustomAttribute<System.Reflection.AssemblyInformationalVersionAttribute>();

            return attribute == null ? "" : "v" + attribute.InformationalVersion;
        }

        public void RunFile(string filename)
        {
            try
            {
                ModulePathExpression path = new ModulePathExpression(filename);
                GlobalEnvironment globals = new GlobalEnvironment();

                LoadModules(path, globals);

                AbstractObject mainModuleObj = Resolve(globals[path.SymbolicName].Evaluate());

                ModuleInstance mainModule = (ModuleInstance)mainModuleObj;

                AbstractObject mainFuncObj = Resolve(mainModule.Environment["main"].Evaluate());

                RunMainFunction(mainFuncObj);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void RunMainFunction(AbstractObject mainFunc)
        {
            Thunk stdin = new Thunk(new InputObject());
            AbstractObject mainResult = Resolve(mainFunc.Apply(stdin));
            Display(mainResult);
        }

        public void Display(AbstractObject str)
        {
            if (str is SymbolObject)
            {
                Console.WriteLine(str.ToString());
                return;
            }

            Thunk headFn = new Thunk(LambdaUtils.MakeHeadFunction(), new GlobalEnvironment());
            Thunk tailFn = new Thunk(LambdaUtils.MakeTailFunction(), new GlobalEnvironment());

            Stack<ICommand> stack = new();
            stack.Push(new ResultCommand(str));

            while (stack.Count > 0)
            {
                str = Resolve(stack.Pop());

                if (str is SymbolObject)
                {
                    Console.Write(str.ToString());
                }
                else
                {
                    stack.Push(str.Apply(tailFn));
                    stack.Push(str.Apply(headFn));
                }
            }

            Console.WriteLine();
        }

        public AbstractObject Resolve(ICommand rootCommand)
        {
            Stack<ICommand> commandStack = new();
            Stack<AbstractObject> resultStack = new();

            commandStack.Push(rootCommand);

            while (commandStack.Count > 0)
                commandStack.Pop().Run(commandStack, resultStack);

            if (resultStack.Count != 1)
                throw new LambdaInterpreterException("Result stack does not contain the expected number of results.");

            return resultStack.Pop();
        }

        public void LoadModules(ModulePathExpression rootPath, GlobalEnvironment genv)
        {
            if (genv.Contains(rootPath))
                return;

            Parser parser = new();

            Stack<ModulePathExpression> pathsToLoad = new();
            pathsToLoad.Push(rootPath);

            List<ModuleInstance> newlyLoadedModules = new List<ModuleInstance>();

            while (pathsToLoad.Count > 0)
            {
                ModulePathExpression path = pathsToLoad.Pop();

                if (genv.Contains(path))
                    continue;

                Module module = parser.ParseModule(path);
                
                ModuleInstance mobj = genv.Register(module);
                newlyLoadedModules.Add(mobj);

                List<ModulePathExpression> subpaths = new List<ModulePathExpression>();
                module.Collect(subpaths);

                foreach (ModulePathExpression subpath in subpaths)
                    pathsToLoad.Push(subpath);
            }

            foreach(ModuleInstance currentInstance in newlyLoadedModules)
            {
                foreach(ModulePathExpression importPath in currentInstance.Module.Imports)
                {
                    ModuleInstance importedInstance = genv[importPath];
                    currentInstance.Environment.Import(importedInstance.Environment);
                }
            }
        }

        public string GetParentFolder(string filename)
        {
            return new FileInfo(filename).DirectoryName ?? throw new ArgumentException("Unable to retrieve parent folder of given file.");
        }
    }
}
