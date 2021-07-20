using System.Collections.Generic;

namespace LoxLangInCSharp
{
    public class Instance
    {
        private readonly Klass klass = null;

        public Instance(Klass klass)
        {
            this.klass = klass;
        }

        public override string ToString()
        {
            return $"{klass.ToString()} instance";
        }
    }
}