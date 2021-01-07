using System;
using System.Collections.Generic;

namespace LoxLangInCSharp
{
    public abstract class Statement
    {
        public interface IVisitor<T>
        {
            public T VisitExpressionStatement(Expression statement);
            public T VisitPrintStatement(Print statement);
        }
        public class Expression : Statement
        {
            public Expression(LoxLangInCSharp.Expression expression)
            {
                this.expression = expression;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitExpressionStatement(this);
            }

            public readonly LoxLangInCSharp.Expression expression;
        }
        public class Print : Statement
        {
            public Print(LoxLangInCSharp.Expression expression)
            {
                this.expression = expression;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitPrintStatement(this);
            }

            public readonly LoxLangInCSharp.Expression expression;
        }

        public abstract T Accept<T>(IVisitor<T> visitor);
    }
}
