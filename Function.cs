using System.Collections.Generic;

namespace LoxLangInCSharp
{
    public class Function : Callable
    {
        private readonly Statement.Function declaration = null;

        public Function(Statement.Function declaration)
        {
            this.declaration = declaration;
        }

        public int Arity()
        {
            return declaration.parameters.Count;
        }

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            Environment environment = new Environment(interpreter.globals);

            for (int i = 0; i < declaration.parameters.Count; i++)
            {
                environment.Define(declaration.parameters[i].lexeme, arguments[i]);
            }

            interpreter.ExecuteBlock(declaration.body, environment);
            return null;
        }

        public override string ToString()
        {
            return $"<fn {declaration.name.lexeme}>";
        }
    }
}