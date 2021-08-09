using System.Collections.Generic;

namespace LoxLangInCSharp
{
    public class Function : Callable
    {
        private readonly Statement.Function declaration = null;
        private readonly Environment closure = null;
        private readonly bool isInitialiser = false;

        public Function(Statement.Function declaration, Environment closure, bool isInitialiser)
        {
            this.declaration = declaration;
            this.closure = closure;
            this.isInitialiser = isInitialiser;
        }

        public Function Bind(Instance instance)
        {
            Environment environment = new Environment(closure);
            environment.Define("this", instance);
            return new Function(declaration, environment, isInitialiser);
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
                if (isInitialiser) {
                    return closure.GetAt(0, "this");
                }
                
                return value.value;
            }

            if (isInitialiser)
            {
                return closure.GetAt(0, "this");
            }

            return null;
        }

        public override string ToString()
        {
            return $"<fn {declaration.name.lexeme}>";
        }
    }
}