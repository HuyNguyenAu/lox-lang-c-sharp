using System.Collections.Generic;

namespace LoxLangInCSharp
{
    public class Klass : Callable
    {
        private readonly string name;
        private readonly Klass superClass;
        private readonly Dictionary<string, Function> methods;

        public Klass(string name, Klass superClass, Dictionary<string, Function> methods)
        {
            this.name = name;
            this.superClass = superClass;
            this.methods = methods;
        }

        public Function FindMethod(string name)
        {
            if (methods.ContainsKey(name))
            {
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

            Function initialiser = FindMethod("init");

            if (initialiser != null)
            {
                initialiser.Bind(instance).Call(interpreter, arguments);
            }

            return instance;
        }

        public int Arity()
        {
            Function initialiser = FindMethod("init");

            if (initialiser == null)
            {
                return 0;
            }

            return initialiser.Arity();
        }

    }
}