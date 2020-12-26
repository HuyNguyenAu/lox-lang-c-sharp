using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Tools
{
    public static class GenerateAst
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("Usage: generate_ast <output directory>");
                Environment.Exit(64);
            }

            string outputDir = args[0];
            DefineAst(outputDir, "Expression", new List<string>()
                {
                    "Binary   : Expression left, Token op, Expression right",
                    "Grouping : Expression expression",
                    "Literal  : object value",
                    "Unary    : Token op, Expression right"
                }
            );
        }

        private static void DefineAst(string outputDir, string baseName, List<string> types)
        {
            string path = $"{outputDir}/{baseName}.cs";
            using StreamWriter writer = new StreamWriter(path);
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine();
            writer.WriteLine("namespace loxlang {");
            writer.WriteLine($"public abstract class {baseName} {{");

            DefineVisitor(writer, baseName, types);

            foreach (string type in types)
            {
                string className = type.Split(":")[0].Trim();
                string fields = type.Split(":")[1].Trim();
                DefineType(writer, baseName, className, fields);
            }

            //The base accept() method.
            writer.WriteLine();
            writer.WriteLine($"public abstract T Accept<T> (IVisitor<T> visitor);");
            writer.WriteLine("}");

            writer.WriteLine("}");
            writer.Close();
        }

        private static void DefineVisitor(StreamWriter writer, string baseName, List<string> types)
        {
            writer.WriteLine($"public interface IVisitor<T> {{");

            foreach (string type in types)
            {
                string typeName = type.Split(':')[0].Trim();
                writer.WriteLine($"public T Visit{typeName}{baseName} ({typeName} {baseName.ToLower()});");
            }

            writer.WriteLine("}");
        }

        private static void DefineType(StreamWriter writer, string baseName, string className, string fieldList)
        {
            writer.WriteLine($"public class {className} : {baseName} {{");
            //Constructor
            writer.WriteLine($"public {className} ({fieldList}) {{");

            // Store parameters.
            string[] fields = fieldList.Split(", ");
            foreach (string field in fields)
            {
                string name = field.Split(" ")[1];
                writer.WriteLine($"this.{name} = {name};");
            }

            writer.WriteLine("}");

            // Visitor pattern.
            writer.WriteLine();
            writer.WriteLine($"public override T Accept<T> (IVisitor<T> visitor) {{");
            writer.WriteLine($"return visitor.Visit{className}{baseName}(this);");
            writer.WriteLine("}");

            // Fields.
            writer.WriteLine();
            foreach (string field in fields)
            {
                writer.WriteLine($"public readonly {field};");
            }

            writer.WriteLine("}");
        }
    }
}
