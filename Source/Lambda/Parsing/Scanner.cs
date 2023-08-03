using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Lambda.Parsing
{
    internal class Scanner
    {
        public string FileName { get; private set; }
        public int LineNumber { get; private set; }
        public string Line { get; private set; }
        public int Index { get; private set; }
        public bool EndOfFile { get; private set; }

        private TextReader _reader;

        public Scanner()
        {
            _reader = Console.In;

            this.FileName = "<stdin>";
            this.LineNumber = 0;
            this.Line = "";
            this.Index = 0;
            this.EndOfFile = false;
        }

        public Scanner(string fileName, string content)
        {
            _reader = new StringReader(content);

            this.FileName = fileName;
            this.LineNumber = 0;
            this.Line = "";
            this.Index = 0;
            this.EndOfFile = false;
        }

        public Scanner(FileInfo file)
        {
            _reader = new StringReader(File.ReadAllText(file.FullName));

            this.FileName = file.FullName;
            this.LineNumber = 0;
            this.Line = "";
            this.Index = 0;
            this.EndOfFile = false;
        }

        public char Peek()
        {
            AdvanceToToken();

            if (EndOfFile)
                return '\0';

            return Line[Index];
        }

        public void Match(char expected)
        {
            if (Peek() != expected)
            {
                if (expected == '\0')
                    ThrowParseException("<eof>");
                else
                    ThrowParseException("'" + expected + "'");
            }

            Index++;
        }

        public string Match(Regex pattern, string expected)
        {
            AdvanceToToken();

            if (EndOfFile)
                ThrowParseException(expected);

            Match match = pattern.Match(Line, Index);

            if (!match.Success)
                ThrowParseException(expected);

            Index += match.Length;

            return match.Value;
        }

        private void AdvanceToToken()
        {
            SkipWhitespace();

            while (!EndOfFile && Index >= Line.Length)
            {
                string? nextLine;
                
                if ((nextLine = _reader.ReadLine()) != null)
                {
                    LineNumber++;
                    Line = nextLine;
                    Index = 0;
                    SkipWhitespace();
                }
                else
                {
                    EndOfFile = true;
                }
            }
        }

        private void SkipWhitespace()
        {
            while (Index < Line.Length && char.IsWhiteSpace(Line[Index]))
                Index++;

            if (Index < Line.Length && Line[Index] == '`')
                Index = Line.Length;
        }

        private void ThrowParseException(string expected)
        {
            string message = "Parsing Error\n" +
                             FileName + "\n" +
                             "Line " + LineNumber + "\n" +
                             Line + "\n" +
                             new string(' ', Index) + "^\n" +
                             (EndOfFile ? "End Of File\n" : "") + 
                             "Expected: " + expected + "\n";

            throw new ParseException(message);
        }
    }
}
