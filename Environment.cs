using System.Collections.Generic;

namespace LoxLangInCSharp
{
    public class Environment
    {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public object Get(Token name)
        {
            if (values.ContainsKey(name.lexeme))
            {
                return values[name.lexeme];
            }

            throw new RuntimeError(name, $"Undefined variable '{name.lexeme}'.");
        }

        public void Assign(Token name, object value)
        {
            if (values.ContainsKey(name.lexeme))
            {
                Put(name.lexeme, value);
            }
            else
            {
                throw new RuntimeError(name, $"Undefined variable '{name.lexeme}'.");
            }
        }

        public void Define(string name, object value)
        {
            Put(name, value);
        }

        // Equivalent to Java's Hash Map 'put' function.
        private void Put(string key, object value)
        {
            if (values.ContainsKey(key))
            {
                values[key] = value;
            }
            else
            {
                values.Add(key, value);
            }
        }
    }
}
