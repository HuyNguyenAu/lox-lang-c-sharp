using System;
using System.Collections.Generic;
using System.IO;

using Tools;

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
                Environment.Exit(64);
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
                if (hadError) Environment.Exit(65);
                if (hadRuntimeError) Environment.Exit(70);
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
            Expression expression = parser.Parse();

            // Stop if we run into an error.
            if (hadError) return;

            interpreter.Interpret(expression);
        }

        public static void Error(int line, string message)
        {
            Report(line, string.Empty, message);
        }

        private static void Report(int line, string where, string message)
        {
            Console.WriteLine($"[line {line}] Error {where}: {message}");
        }

        public static void RuntimeError(RuntimeError error)
        {
            Console.Error.WriteLine($"{error.GetMessage()}{Environment.NewLine}" +
                $"[line {error.GetToken().line}]");
            hadRuntimeError = true;
        }
    }
}
