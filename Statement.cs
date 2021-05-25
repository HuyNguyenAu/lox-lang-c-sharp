using System;
using System.Collections.Generic;

namespace LoxLangInCSharp
{
    public abstract class Statement
    {
        public interface IVisitor<T>
        {
            public T VisitBlockStatement(Block statement);
            public T VisitExpressionStatement(Expression statement);
            public T VisitPrintStatement(Print statement);
            public T VisitVarStatement(Var statement);
        }
        public class Block : Statement
        {
            public Block(List<LoxLangInCSharp.Statement> statements)
            {
                this.statements = statements;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitBlockStatement(this);
            }

            public readonly List<LoxLangInCSharp.Statement> statements;
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
        public class Var : Statement
        {
            public Var(Token name, LoxLangInCSharp.Expression initialiser)
            {
                this.name = name;
                this.initialiser = initialiser;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitVarStatement(this);
            }

            public readonly Token name;
            public readonly LoxLangInCSharp.Expression initialiser;
        }

        public abstract T Accept<T>(IVisitor<T> visitor);
    }
}
