using System.Collections.Generic;

namespace LoxLangInCSharp
{
    public interface Callable
    {
        int Arity();
        object Call(Interpreter interpreter, List<object> arguments);
    }
}