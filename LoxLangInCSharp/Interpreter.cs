using System;
using System.Collections.Generic;
using System.Text;

namespace LoxLangInCSharp
{
    public class Interpreter : Expression.IVisitor<object>, Statement.IVisitor<object>
    {
        public object VisitBinaryExpression(Expression.Binary expression)
        {
            object left = Evaluate(expression.left);
            object right = Evaluate(expression.right);

            switch (expression.op.type)
            {
                case TokenType.GREATER:
                    CheckNumberOperands(expression.op, left, right);
                    return (double)left > (double)right;

                case TokenType.GREATER_EQUAL:
                    CheckNumberOperands(expression.op, left, right);
                    return (double)left >= (double)right;

                case TokenType.LESS:
                    CheckNumberOperands(expression.op, left, right);
                    return (double)left < (double)right;

                case TokenType.LESS_EQUAL:
                    CheckNumberOperands(expression.op, left, right);
                    return (double)left <= (double)right;

                case TokenType.MINUS:
                    CheckNumberOperands(expression.op, left, right);
                    return (double)left - (double)right;

                case TokenType.SLASH:
                    CheckNumberOperands(expression.op, left, right);

                    // Handle a zero divisor operation.
                    if ((double)right <= 0d) throw new RuntimeError(expression.op, "Unable to divide by zero.");

                    return (double)left / (double)right;

                case TokenType.STAR:
                    CheckNumberOperands(expression.op, left, right);
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

                    throw new RuntimeError(expression.op, 
                        "Operands must be two numbers or two strings.");

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
                    CheckNumberOperand(expression.op, right);
                    return -(double)right;
                case TokenType.BANG:
                    return !IsTruthy(right);
            }

            // Unreachable.
            return null;
        }

        public object VisitExpressionStatement(Statement.Expression statement)
        {
            Evaluate(statement.expression);
            return null;
        }

        public object VisitPrintStatement(Statement.Print statement)
        {
            object value = Evaluate(statement.expression);
            Console.WriteLine(Stringify(value));
            return null;
        }

        private void CheckNumberOperand(Token op, object operand)
        {
            if (operand.GetType() == typeof(double)) return;

            throw new RuntimeError(op, "Operand must be a number.");
        }

        private void CheckNumberOperands(Token op, object left, object right)
        {
            if (left.GetType() == typeof(double) &&
                right.GetType() == typeof(double)) return;

            throw new RuntimeError(op, "Operand must be a numbers.");
        }

        private object Evaluate(Expression expression)
        {
            return expression.Accept(this);
        }

        private void Execute(Statement statement)
        {
            statement.Accept(this);
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

        private string Stringify(object obj)
        {
            if (obj == null) return "nil";

            if (obj.GetType() == typeof(double))
            {
                string text = obj.ToString();
                
                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }

                return text;
            }

            return obj.ToString();
        }

        public void Interpret(List<Statement> statements)
        {
            try
            {
                foreach (Statement statement in statements)
                {
                    Execute(statement);
                }
            }
            catch (RuntimeError error)
            {
                Program.RuntimeError(error);
            }
        }
    }
}
