using System;
using System.Collections.Generic;
using System.IO;

namespace LoxLangInCSharp
{
    class Program
    {
        static readonly Interpreter interpreter = new Interpreter();
        static bool hadError = false;
        static bool hadRuntimeError = false;

        public static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: jlox [script]");
                System.Environment.Exit(64);
            }
            else if (args.Length == 1)
            {
                RunFile(args[0]);
            }
            else
            {
                RunPrompt();
            }
        }

        private static void RunFile(string path)
        {
            try
            {
                Run(File.ReadAllText(path));
                if (hadError) System.Environment.Exit(65);
                if (hadRuntimeError) System.Environment.Exit(70);
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
        }

        private static void RunPrompt()
        {
            while (true)
            {
                try
                {
                    Console.Write("> ");
                    string line = Console.ReadLine();
                    if (string.IsNullOrEmpty(line)) break;
                    Run(line);
                    hadError = false;
                }
                catch (IOException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static void Run(string source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();
            Parser parser = new Parser(tokens);
            List<Statement> statements = new List<Statement>();

            // TEMP: Handle parse errors.
            try
            {
                statements = parser.Parse();
            }
            catch (Exception) { 
                hadError = true;
            }

            // Stop if we run into an error.
            if (hadError) return;
            // TEMP: Exit early when expression is invalid.
            if (statements.Count <= 0) return;

            // What to do if a statement is null ? Should we just exit?
            foreach (Statement statement in statements)
            {
                if (statement == null) return;
            }

            Resolver resolver = new Resolver(interpreter);
            resolver.Resolve(statements);

            // Stop if there was a resolution error.
            if (hadError) return;

            // Handle ReturnException.
            try
            {
                interpreter.Interpret(statements);
            }
            catch (Exception)
            {
                System.Environment.Exit(65);
            }
            
        }

        public static void Error(int line, string message)
        {
            Report(line, string.Empty, message);
        }

        public static void Error(Token token, string message)
        {
            if (token.type == TokenType.EOF)
            {
                Report(token.line, " at end", message);
            }
            else
            {
                Report(token.line, $" at '{token.lexeme}'", message);
            }
        }

        private static void Report(int line, string where, string message)
        {
            Console.WriteLine($"[line {line}] Error {where}: {message}");
            hadError = true;
        }

        public static void RuntimeError(RuntimeError error)
        {
            Console.Error.WriteLine($"[line {error.GetToken().line + 1}]: {error.GetMessage()}{System.Environment.NewLine}");
            hadRuntimeError = true;
        }
    }
}

