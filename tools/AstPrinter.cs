using LoxLangInCSharp;
using System;
using System.Text;

namespace Tools
{
    public class AstPrinter : Expression.IVisitor<string>
    {
        public string Print(Expression expr)
        {
            return expr.Accept(this);
        }

        public string VisitBinaryExpression(Expression.Binary expression)
        {
            return Parenthesize(expression.op.lexeme, expression.left, expression.right);
        }

        public string VisitGroupingExpression(Expression.Grouping expression)
        {
            return Parenthesize("group", expression.expression);
        }

        public string VisitLiteralExpression(Expression.Literal expression)
        {
            if (expression.value == null) return "nil";
            return expression.value.ToString();
        }

        public string VisitUnaryExpression(Expression.Unary expression)
        {
            return Parenthesize(expression.op.lexeme, expression.right);
        }

        private string Parenthesize(string name, params Expression[] expressions)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append('(').Append(name);

            foreach (Expression expression in expressions)
            {
                builder.Append(' ');
                builder.Append(expression.Accept(this));
            }

            builder.Append(')');

            return builder.ToString();
        }

        // Hack to test printer.
        static void Main(string[] args)
        {
            Expression expression = new Expression.Binary(
                    new Expression.Unary(
                            new Token(TokenType.MINUS, "-", null, 1),
                            new Expression.Literal(123)),
                    new Token(TokenType.STAR, "*", null, 1),
                    new Expression.Grouping(new Expression.Literal(45.67))
            );

            Console.WriteLine(new AstPrinter().Print(expression));
        }

        public string VisitVariableExpression(Expression.Variable expression)
        {
            throw new NotImplementedException();
        }

        public string VisitAssignExpression(Expression.Assign expression)
        {
            throw new NotImplementedException();
        }

        public string VisitLogicalExpression(Expression.Logical expression)
        {
            throw new NotImplementedException();
        }

        public string VisitCallExpression(Expression.Call expression)
        {
            throw new NotImplementedException();
        }

        public string VisitGetExpression(Expression.Get expression)
        {
            throw new NotImplementedException();
        }

        public string VisitSetExpression(Expression.Set expression)
        {
            throw new NotImplementedException();
        }

        public string VisitThisExpression(Expression.This expression)
        {
            throw new NotImplementedException();
        }
    }
}
