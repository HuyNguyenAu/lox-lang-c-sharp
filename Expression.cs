using System;
using System.Collections.Generic;

namespace LoxLangInCSharp
{
    public abstract class Expression
    {
        public interface IVisitor<T>
        {
            public T VisitAssignExpression(Assign expression);
            public T VisitBinaryExpression(Binary expression);
            public T VisitCallExpression(Call expression);
            public T VisitGetExpression(Get expression);
            public T VisitGroupingExpression(Grouping expression);
            public T VisitLiteralExpression(Literal expression);
            public T VisitLogicalExpression(Logical expression);
            public T VisitThisExpression(This expression);
            public T VisitSetExpression(Set expression);
            public T VisitUnaryExpression(Unary expression);
            public T VisitVariableExpression(Variable expression);
        }
        public class Assign : Expression
        {
            public Assign(Token name, Expression value)
            {
                this.name = name;
                this.value = value;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitAssignExpression(this);
            }

            public readonly Token name;
            public readonly Expression value;
        }
        public class Binary : Expression
        {
            public Binary(Expression left, Token op, Expression right)
            {
                this.left = left;
                this.op = op;
                this.right = right;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitBinaryExpression(this);
            }

            public readonly Expression left;
            public readonly Token op;
            public readonly Expression right;
        }
        public class Call : Expression
        {
            public Call(Expression callee, Token parenthesis, List<Expression> arguments)
            {
                this.callee = callee;
                this.parenthesis = parenthesis;
                this.arguments = arguments;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitCallExpression(this);
            }

            public readonly Expression callee;
            public readonly Token parenthesis;
            public readonly List<Expression> arguments;
        }
        public class Get : Expression
        {
            public Get(Expression obj, Token name)
            {
                this.obj = obj;
                this.name = name;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitGetExpression(this);
            }

            public readonly Expression obj;
            public readonly Token name;
        }
        public class Grouping : Expression
        {
            public Grouping(Expression expression)
            {
                this.expression = expression;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitGroupingExpression(this);
            }

            public readonly Expression expression;
        }
        public class Literal : Expression
        {
            public Literal(object value)
            {
                this.value = value;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitLiteralExpression(this);
            }

            public readonly object value;
        }
        public class Logical : Expression
        {
            public Logical(Expression left, Token op, Expression right)
            {
                this.left = left;
                this.op = op;
                this.right = right;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitLogicalExpression(this);
            }

            public readonly Expression left;
            public readonly Token op;
            public readonly Expression right;
        }
        public class This : Expression
        {
            public This(Token keyword)
            {
                this.keyword = keyword;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitThisExpression(this);
            }

            public readonly Token keyword;
        }
        public class Set : Expression
        {
            public Set(Expression obj, Token name, Expression value)
            {
                this.obj = obj;
                this.name = name;
                this.value = value;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitSetExpression(this);
            }

            public readonly Expression obj;
            public readonly Token name;
            public readonly Expression value;
        }
        public class Unary : Expression
        {
            public Unary(Token op, Expression right)
            {
                this.op = op;
                this.right = right;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitUnaryExpression(this);
            }

            public readonly Token op;
            public readonly Expression right;
        }
        public class Variable : Expression
        {
            public Variable(Token name)
            {
                this.name = name;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitVariableExpression(this);
            }

            public readonly Token name;
        }

        public abstract T Accept<T>(IVisitor<T> visitor);
    }
}
