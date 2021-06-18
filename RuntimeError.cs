using System;

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

        public Token GetToken()
        {
            return token;
        }
        
        public string GetMessage()
        {
            return message;
        }
    }
}
