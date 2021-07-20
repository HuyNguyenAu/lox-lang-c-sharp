using System.Collections.Generic;

namespace LoxLangInCSharp
{
    public class Klass : Callable
    {
        private readonly string name;

        public Klass(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }

        public object Call(Interpreter interpreter, List<object> arguments)
        {
            Instance instance = new Instance(this);
            return instance;
        }

        public int Arity()
        {
            return 0;
        }

    }
}