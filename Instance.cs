using System.Collections.Generic;

namespace LoxLangInCSharp
{
    public class Instance
    {
        private readonly Klass klass = null;
        private readonly Dictionary<string, object> fields = new Dictionary<string, object>();

        public Instance(Klass klass)
        {
            this.klass = klass;
        }

        public object Get(Token name)
        {
            if (fields.ContainsKey(name.lexeme))
            {
                return fields[name.lexeme];
            }

            Function method = klass.FindMethod(name.lexeme);
            if (method != null) {
                return method;
            }

            throw new RuntimeError(name, $"Undefined property '{name.lexeme}'.");
        }

        public void Set(Token name, object  value)
        {
            fields.Add(name.lexeme, value);
        }

        public override string ToString()
        {
            return $"{klass.ToString()} instance";
        }
    }
}