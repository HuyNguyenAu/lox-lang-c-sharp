using System.Collections.Generic;

namespace LoxLangInCSharp
{
    public class Function : Callable
    {
        private readonly Statement.Function declaration = null;
        private readonly Environment closure = null;

        public Function(Statement.Function declaration, Environment closure)
        {
            this.declaration = declaration;
            this.closure = closure;
        }

        public int Arity()
        {
            return declaration.parameters.Count;
        }

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            Environment environment = new Environment(closure);

            for (int i = 0; i < declaration.parameters.Count; i++)
            {
                environment.Define(declaration.parameters[i].lexeme, arguments[i]);
            }

            try
            {
                interpreter.ExecuteBlock(declaration.body, environment);
            }
            catch (ReturnException value)
            {
                return value.value;
            }

            return null;
        }

        public override string ToString()
        {
            return $"<fn {declaration.name.lexeme}>";
        }
    }
}