using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.IO;
using Lambda.AST;
using System.Runtime.CompilerServices;

namespace Lambda.Parsing
{
    internal class Parser
    {
        private static Regex _whitespace = new(@"\G\s+");
        private static Regex _comment = new(@"\G`[^\n]*");
        private static Regex _identifier = new(@"\G[^\s\(\)\[\]\{\};:'"",\.`]+");
        private static Regex _quotedSymbol = new(@"\G'[^\n']*'");
        private static Regex _stringLiteral = new(@"\G""[^\n""]*""");
        private static Regex _unicodeEscape = new(@"\\u(\d+);");
        private static Regex _modulePath = new(@"\G\[[^\[\]]*\]");

        private Scanner _scanner;
        private string _workingFolder;

        public Parser()
        {
            SetSource("", new Scanner("", ""));
        }

        public Parser(string workingFolder, Scanner scanner)
        {
            SetSource(workingFolder, scanner);
        }

        [MemberNotNull(nameof(_scanner))]
        [MemberNotNull(nameof(_workingFolder))]
        public void SetSource(string workingFolder, Scanner scanner)
        {
            _workingFolder = workingFolder;
            _scanner = scanner;
        }

        public ShellStatement ParseShellStatement()
        {
            if (Peek() == ':')
            {
                Match(':');
                ModulePathExpression modulePath = ParseModulePathExpression();
                return new ModuleImportShellStatement(modulePath);
            }
            else
            {
                AbstractExpression expression = ParseExpression();
                SymbolExpression? symbol = expression as SymbolExpression;

                if (symbol != null && !symbol.IsQuoted && Peek() != ';')
                {
                    AbstractExpression value = ParseExpression();
                    Match(';');

                    Definition definition = new Definition(symbol.Name, value);
                    return new DefinitionShellStatement(definition);
                }
                else
                {
                    Match(';');
                    return new ExpressionShellStatement(expression);
                }
            }
        }

        public Module ParseModule(ModulePathExpression path)
        {
            string source = File.ReadAllText(path.CanonicalPath);

            SetSource(path.ParentFolder, new Scanner(new FileInfo(path.CanonicalPath)));
            
            List<ModulePathExpression> imports = new();
            List<Definition> definitions = new();

            while (Peek() != '\0')
            {
                if (Peek() == ':')
                {
                    Match(':');
                    imports.Add(ParseModulePathExpression());
                }
                else
                {
                    definitions.Add(ParseDefinition());
                }
            }

            return new Module(path, imports, definitions);
        }

        public Definition ParseDefinition()
        {
            string name = ParseIdentifier();

            AbstractExpression value = ParseExpression();
            Match(';');
            return new Definition(name, value);
        }

        public AbstractExpression ParseExpression()
        {
            return ParseConditionalSuffixExpression();
        }

        public AbstractExpression ParseConditionalSuffixExpression()
        {
            AbstractExpression result = ParseFieldSuffixExpression();

            if (Peek() == '=')
            {
                AbstractExpression left = result;
                Match('=');
                AbstractExpression right = ParseExpression();
                Match('?');
                AbstractExpression equalResult = ParseExpression();
                Match(':');
                AbstractExpression distinctResult = ParseExpression();

                result = new ConditionalExpression(left, right, equalResult, distinctResult);
            }

            return result;
        }

        public AbstractExpression ParseFieldSuffixExpression()
        {
            AbstractExpression result = ParseRootExpression();

            while (Peek() == '.')
            {
                Match('.');
                string identifier = ParseIdentifier();
                result = new CallExpression(result, new SymbolExpression(identifier, true));
            }

            return result;
        }

        public AbstractExpression ParseRootExpression()
        {
            switch (Peek())
            {
                case '[': return ParseModulePathExpression();
                case ',':  return ParseLambda();
                case '(':  return ParseCall();
                case '\"': return ParseString();
                default: return ParseSymbol();
            }
        }

        public ModulePathExpression ParseModulePathExpression()
        {
            string modulePath = Match(_modulePath, "module path expression");
            modulePath = modulePath.Substring(1, modulePath.Length - 2);
            modulePath = Path.Combine(_workingFolder, modulePath);
            ModulePathExpression expr = new ModulePathExpression(modulePath);
            return expr;
        }

        public LambdaExpression ParseLambda()
        {
            Match(',');

            string parameter = ParseIdentifier();
            AbstractExpression body = ParseExpression();

            return new LambdaExpression(parameter, body);
        }

        public CallExpression ParseCall()
        {
            Match('(');

            CallExpression result = new CallExpression(ParseExpression(), ParseExpression());

            while (Peek() != ')')
                result = new CallExpression(result, ParseExpression());

            Match(')');

            return result;
        }

        public AbstractExpression ParseString()
        {
            string str = Match(_stringLiteral, "string literal");
            str = FormatLiteral(str);
            return LambdaUtils.StringToLambda(str);
        }

        public SymbolExpression ParseSymbol()
        {
            if (Peek() == '\'')
            {
                string name = Match(_quotedSymbol, "quoted symbol");
                name = FormatLiteral(name);
                return new SymbolExpression(name, true);
            }
            else
            {
                return new SymbolExpression(ParseIdentifier(), false);
            }
        }

        public string ParseIdentifier()
        {
            return Match(_identifier, "identifier");
        }

        public char Peek()
        {
            return _scanner.Peek();
        }

        public void Match(char expected)
        {
            _scanner.Match(expected);
        }

        public string Match(Regex regex, string expected)
        {
            return _scanner.Match(regex, expected);
        }

        public static string FormatLiteral(string raw)
        {
            raw = raw.Substring(1, raw.Length - 2)
                      .Replace("\\t", "\t")
                      .Replace("\\r", "\r")
                      .Replace("\\n", "\n")
                      .Replace("\\q", "\'")
                      .Replace("\\Q", "\"")
                      .Replace("\\b", "\\");

            Match m = _unicodeEscape.Match(raw);

            while (m.Success)
            {
                string escapeCode = m.Value;
                int codepoint = int.Parse(m.Groups[1].Value);
                string chr = char.ConvertFromUtf32(codepoint);
                raw = raw.Replace(escapeCode, chr);
                m = _unicodeEscape.Match(raw);
            }

            return raw;
        }
    }
}
