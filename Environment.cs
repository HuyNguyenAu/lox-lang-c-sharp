using System.Collections.Generic;

namespace LoxLangInCSharp
{
    public class Environment
    {
        private readonly Environment enclosing = null;
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        /* Used for global scope enviroment. No enclosing
        since it is at the end of the parent pointer tree. */
        public Environment() { }

        /* Create a new local scope nested inside the enclosing
        outer one. */
        public Environment(Environment enclosing) {
            this.enclosing = enclosing;
        }

        public object Get(Token name)
        {
            if (values.ContainsKey(name.lexeme))
            {
                return values[name.lexeme];
            }

            if (enclosing != null) return enclosing.Get(name);

            throw new RuntimeError(name, $"Undefined variable '{name.lexeme}'.");
        }

        public void Assign(Token name, object value)
        {
            if (values.ContainsKey(name.lexeme))
            {
                values[name.lexeme] = value;
                return;
            }

            if (enclosing != null) {
                enclosing.Assign(name, value);
                return;
            }

            throw new RuntimeError(name, $"Undefined variable '{name.lexeme}'.");
        }

        public void Define(string name, object value)
        {
            values[name] = value;
        }

        public object GetAt(int distance, string name)
        {
            return Ancestor(distance).values[name];
        }

        public void AssignAt(int distance, Token name, object value)
        {
            Ancestor(distance).values[name.lexeme] = value;
        }

        public Environment Ancestor(int distance)
        {
            Environment environment = this;
            for (int i = 0; i < distance; i++)
            {
                environment = environment.enclosing;
            }

            return environment;
        }
    }
}
