using System;
using System.Collections.Generic;
using System.Text;

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

        public void Define(string name, object value)
        {
            values.Add(name, value);
        }
    }
}
