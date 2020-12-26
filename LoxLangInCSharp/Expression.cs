﻿using System;
using System.Collections.Generic;

namespace LoxLangInCSharp
{
    public abstract class Expression
    {
        public interface IVisitor<T>
        {
            public T VisitBinaryExpression(Binary expression);
            public T VisitGroupingExpression(Grouping expression);
            public T VisitLiteralExpression(Literal expression);
            public T VisitUnaryExpression(Unary expression);
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

        public abstract T Accept<T>(IVisitor<T> visitor);
    }
}