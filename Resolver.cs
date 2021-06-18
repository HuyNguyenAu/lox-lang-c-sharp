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

        private void Declare(Token name)
        {
            if (scopes.Count <= 0) return;

            Dictionary<string, bool> scope = scopes.Peek();
            Put(name.lexeme, false, scope);
        }

        private void Define(Token name)
        {
            if (scopes.Count <= 0) return;
            Put(name.lexeme, true, scopes.Peek());
        }

        private void ResolveLocal(Expression expression, Token name)
        {
            // In C# we can't access a stack using indexes. But we can in Java.
            List<Dictionary<string, bool>> temp = new List<Dictionary<string, bool>>();
            temp.AddRange(scopes.ToArray());

            for (int i = temp.Count - 1; i >= 0; i--)
            {
                if (temp[i].ContainsKey(name.lexeme))
                {
                    interpreter.Resolve(expression, temp.Count - 1 - i);
                    return;
                }
            }
        }

        private void Resolve(Expression expression)
        {
            expression.Accept(this);
        }

        public object VisitAssignExpression(Expression.Assign expression)
        {
            return null;
        }

        public object VisitBinaryExpression(Expression.Binary expression)
        {
            return null;
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
            return null;
        }

        public object VisitCallExpression(Expression.Call expression)
        {
            return null;
        }

        public object VisitContinueStatement(Statement.Continue statement)
        {
            return null;
        }

        public object VisitExpressionStatement(Statement.Expression statement)
        {
            return null;
        }

        public object VisitFunctionStatement(Statement.Function statement)
        {
            return null;
        }

        public object VisitGroupingExpression(Expression.Grouping expression)
        {
            return null;
        }

        public object VisitIfStatement(Statement.If statement)
        {
            return null;
        }

        public object VisitLiteralExpression(Expression.Literal expression)
        {
            return null;
        }

        public object VisitLogicalExpression(Expression.Logical expression)
        {
            return null;
        }

        public object VisitPrintStatement(Statement.Print statement)
        {
            return null;
        }

        public object VisitReturnStatement(Statement.Return statement)
        {
            return null;
        }

        public object VisitUnaryExpression(Expression.Unary expression)
        {
            return null;
        }

        public object VisitVariableExpression(Expression.Variable expression)
        {
            if (scopes.Count > 0 && scopes.Peek()[expression.name.lexeme] == false)
            {
                Program.Error(expression.name, "Can't read local variable in its own initializer.");
            }

            ResolveLocal(expression, expression.name);
            return null;
        }

        public object VisitVarStatement(Statement.Var statement)
        {
            Declare(statement.name);

            if (statement.initialiser != null)
            {
                Resolve(statement.initialiser);
            }

            Define(statement.name);
            return null;
        }

        public object VisitWhileStatement(Statement.While statement)
        {
            return null;
        }

        // Equivalent to Java's Hash Map 'put' function.
        private void Put(string key, bool value, Dictionary<string, bool> scope)
        {
            if (scope.ContainsKey(key))
            {
                scope[key] = value;
            }
            else
            {
                scope.Add(key, value);
            }
        }
    }
}