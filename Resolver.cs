using System.Collections.Generic;

namespace LoxLangInCSharp {
    public class Resolver : Expression.IVisitor<object>, Statement.IVisitor<object> {
        private readonly Interpreter interpreter = null;
        private readonly Stack<Dictionary<string, bool>> scopes = new Stack<Dictionary<string, bool>>();

        public Resolver(Interpreter interpreter)
        {
            this.interpreter = interpreter;
        }

        private void Resolve(List<Statement> statements)
        {
            foreach (Statement statement in statements)
            {
                Resolve(statement);
            }
        }

        private void Resolve(Statement statement)
        {
            statement.Accept(this);
        }

        private void BeginScope()
        {
            scopes.Push(new Dictionary<string, bool>());
        }

        private void EndScope()
        {
            scopes.Pop();
        }

        private void Resolve(Expression expression)
        {
            expression.Accept(this);
        }

        public object VisitAssignExpression(Expression.Assign expression)
        {
            throw new System.NotImplementedException();
        }

        public object VisitBinaryExpression(Expression.Binary expression)
        {
            throw new System.NotImplementedException();
        }

        public object VisitBlockStatement(Statement.Block statement)
        {
            BeginScope();
            Resolve(statement.statements);
            EndScope();
            return null;
        }

        public object VisitBreakStatement(Statement.Break statement)
        {
            throw new System.NotImplementedException();
        }

        public object VisitCallExpression(Expression.Call expression)
        {
            throw new System.NotImplementedException();
        }

        public object VisitContinueStatement(Statement.Continue statement)
        {
            throw new System.NotImplementedException();
        }

        public object VisitExpressionStatement(Statement.Expression statement)
        {
            throw new System.NotImplementedException();
        }

        public object VisitFunctionStatement(Statement.Function statement)
        {
            throw new System.NotImplementedException();
        }

        public object VisitGroupingExpression(Expression.Grouping expression)
        {
            throw new System.NotImplementedException();
        }

        public object VisitIfStatement(Statement.If statement)
        {
            throw new System.NotImplementedException();
        }

        public object VisitLiteralExpression(Expression.Literal expression)
        {
            throw new System.NotImplementedException();
        }

        public object VisitLogicalExpression(Expression.Logical expression)
        {
            throw new System.NotImplementedException();
        }

        public object VisitPrintStatement(Statement.Print statement)
        {
            throw new System.NotImplementedException();
        }

        public object VisitReturnStatement(Statement.Return statement)
        {
            throw new System.NotImplementedException();
        }

        public object VisitUnaryExpression(Expression.Unary expression)
        {
            throw new System.NotImplementedException();
        }

        public object VisitVariableExpression(Expression.Variable expression)
        {
            throw new System.NotImplementedException();
        }

        public object VisitVarStatement(Statement.Var statement)
        {
            throw new System.NotImplementedException();
        }

        public object VisitWhileStatement(Statement.While statement)
        {
            throw new System.NotImplementedException();
        }
    }
}