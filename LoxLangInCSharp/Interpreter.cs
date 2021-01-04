using System;
using System.Collections.Generic;
using System.Text;

namespace LoxLangInCSharp
{
    public class Interpreter : Expression.IVisitor<object>
    {
        public object VisitBinaryExpression(Expression.Binary expression)
        {
            object left = Evaluate(expression.left);
            object right = Evaluate(expression.right);

            switch (expression.op.type)
            {
                case TokenType.GREATER:
                    return (double)left > (double)right;

                case TokenType.GREATER_EQUAL:
                    return (double)left >= (double)right;

                case TokenType.LESS:
                    return (double)left < (double)right;

                case TokenType.LESS_EQUAL:
                    return (double)left <= (double)right;

                case TokenType.MINUS:
                    return (double)left - (double)right;

                case TokenType.SLASH:
                    return (double)left / (double)right;

                case TokenType.STAR:
                    return (double)left * (double)right;

                case TokenType.PLUS:
                    if (left.GetType() == typeof(double) &&
                        right.GetType() == typeof(double))
                    {
                        return (double)left + (double)right;
                    }

                    if (left.GetType() == typeof(string) &&
                        right.GetType() == typeof(string))
                    {
                        return (string)left + (string)right;
                    }

                    break;

                case TokenType.BANG_EQUAL:
                    return !IsEqual(left, right);
                
                case TokenType.EQUAL_EQUAL:
                    return IsEqual(left, right);
            }

            // Unreachable.
            return null;
        }

        public object VisitGroupingExpression(Expression.Grouping expression)
        {
            return Evaluate(expression.expression);
        }

        public object VisitLiteralExpression(Expression.Literal expression)
        {
            return expression.value;
        }

        public object VisitUnaryExpression(Expression.Unary expression)
        {
            object right = Evaluate(expression.right);

            switch (expression.op.type)
            {
                case TokenType.MINUS:
                    return -(double)right;
                case TokenType.BANG:
                    return !IsTruthy(right);
            }

            // Unreachable.
            return null;
        }

        private object Evaluate(Expression expression)
        {
            return expression.Accept(this);
        }

        private bool IsTruthy(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() == typeof(bool)) return (bool)obj;

            return true;
        }

        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;

            return a.Equals(b);
        }
    }
}
