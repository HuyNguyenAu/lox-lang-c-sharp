using System;
using System.Collections.Generic;
using System.Text;

namespace LoxLangInCSharp
{
    public class RuntimeError : SystemException
    {
        readonly Token token;
        readonly string message;

        public RuntimeError(Token token, string message)
        {
            this.token = token;
            this.message = message;
        }
    }
}
