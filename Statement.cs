using System;
using System.Collections.Generic;

namespace LoxLangInCSharp
{
    public abstract class Statement
    {
        public interface IVisitor<T>
        {
            public T VisitBlockStatement(Block statement);
            public T VisitBreakStatement(Break statement);
            public T VisitExpressionStatement(Expression statement);
            public T VisitIfStatement(If statement);
            public T VisitPrintStatement(Print statement);
            public T VisitVarStatement(Var statement);
            public T VisitWhileStatement(While statement);
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
        public class Break : Statement
        {
            public Break()
            {
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitBreakStatement(this);
            }

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
        public class If : Statement
        {
            public If(LoxLangInCSharp.Expression condition, LoxLangInCSharp.Statement thenBranch, LoxLangInCSharp.Statement elseBranch)
            {
                this.condition = condition;
                this.thenBranch = thenBranch;
                this.elseBranch = elseBranch;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitIfStatement(this);
            }

            public readonly LoxLangInCSharp.Expression condition;
            public readonly LoxLangInCSharp.Statement thenBranch;
            public readonly LoxLangInCSharp.Statement elseBranch;
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
        public class While : Statement
        {
            public While(LoxLangInCSharp.Expression condition, LoxLangInCSharp.Statement body)
            {
                this.condition = condition;
                this.body = body;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitWhileStatement(this);
            }

            public readonly LoxLangInCSharp.Expression condition;
            public readonly LoxLangInCSharp.Statement body;
        }

        public abstract T Accept<T>(IVisitor<T> visitor);
    }
}
