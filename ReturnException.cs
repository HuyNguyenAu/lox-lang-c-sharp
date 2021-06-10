using System;
using System.Collections.Generic;
using System.Text;

namespace LoxLangInCSharp
{
    public class ReturnException : SystemException
    {
        public readonly object value = null;

        public ReturnException(object value)
        {
            this.value = value;
        }
    }
}