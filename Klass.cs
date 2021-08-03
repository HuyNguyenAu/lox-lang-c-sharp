using System.Collections.Generic;

namespace LoxLangInCSharp
{
    public class Klass : Callable
    {
        private readonly string name;
        private readonly Dictionary<string, Function> methods;

        public Klass(string name, Dictionary<string, Function> methods)
        {
            this.name = name;
            this.methods = methods;
        }

        public Function FindMethod(string name) {
            if (methods.ContainsKey(name)) {
                return methods[name];
            }

            return null;
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