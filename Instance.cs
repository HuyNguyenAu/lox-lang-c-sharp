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

            throw new RuntimeError(name, $"Undefined property '{name.lexeme}'.");
        }

        public override string ToString()
        {
            return $"{klass.ToString()} instance";
        }
    }
}