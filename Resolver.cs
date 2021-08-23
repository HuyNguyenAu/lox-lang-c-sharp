using System.Collections.Generic;
using System.Linq;

namespace LoxLangInCSharp
{
    public class Resolver : Expression.IVisitor<object>, Statement.IVisitor<object>
    {
        private readonly Interpreter interpreter = null;
        private readonly Stack<Dictionary<string, bool>> scopes = new Stack<Dictionary<string, bool>>();
        private FunctionType currentFunction = FunctionType.NONE;
        private ClassType currentClass = ClassType.NONE;
        private enum FunctionType
        {
            NONE,
            INITIALISER,
            FUNCTION,
            METHOD
        }
        private enum ClassType
        {
            NONE,
            CLASS,
            SUBCLASS
        }

        public Resolver(Interpreter interpreter)
        {
            this.interpreter = interpreter;
        }

        public void Resolve(List<Statement> statements)
        {
            foreach (Statement statement in statements)
            {
                Resolve(statement);
            }
        }

        private void ResolveFunction(Statement.Function function, FunctionType type)
        {
            FunctionType enclosingFunction = currentFunction;
            currentFunction = type;

            BeginScope();
            foreach (Token parameter in function.parameters)
            {
                Declare(parameter);
                Define(parameter);
            }
            Resolve(function.body);
            EndScope();
            currentFunction = enclosingFunction;
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

            if (scope.ContainsKey(name.lexeme))
            {
                Program.Error(name, "Already a variable with this name in this scope.");
            }

            scope[name.lexeme] = false;
        }

        private void Define(Token name)
        {
            if (scopes.Count <= 0) return;
            scopes.Peek()[name.lexeme] = true;
        }

        private void ResolveLocal(Expression expression, Token name)
        {
            Dictionary<string, bool>[] temp = scopes.ToArray();
            // Dictionary<string, bool>[] tempScopes = scopes.Reverse().ToArray();

            // for (int i = scopes.Count() - 1; i >= 0; i--)
            // {
            //     if (tempScopes[i].ContainsKey(name.lexeme))
            //     {
            //         interpreter.Resolve(expression, scopes.Count() - 1 - i);
            //         return;
            //     }
            // }

            /* Currently the innermost scope is at the top of the scopes stack.
            So we need to go through the innermost to outmost scope to find the
            matching name.
            When we find it, we need to keep track of the numbers of scopes we are
            current on and where it was found.
            Since we started at the top of the scope stack (first item), the 
            distance will just be the number of iterations(i).
            I'm still not sure why the Java implementation has the innermost scope at the top but
            C# doesn't. I expected it to be at the top... */
            for (int i = 0; i < scopes.Count; i++)
            {
                if (temp[i].ContainsKey(name.lexeme))
                {
                    interpreter.Resolve(expression, i);
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
            Resolve(expression.value);
            ResolveLocal(expression, expression.name);
            return null;
        }

        public object VisitBinaryExpression(Expression.Binary expression)
        {
            Resolve(expression.left);
            Resolve(expression.right);
            return null;
        }

        public object VisitBlockStatement(Statement.Block statement)
        {
            BeginScope();
            Resolve(statement.statements);
            EndScope();
            return null;
        }

        public object VisitClassStatement(Statement.Class statement)
        {
            ClassType enclosingClass = currentClass;
            currentClass = ClassType.CLASS;

            Declare(statement.name);
            Define(statement.name);

            if (statement.superclass != null && statement.name.lexeme == statement.superclass.name.lexeme)
            {
                Program.Error(statement.superclass.name.line, "A class can't inherit from itself.");
            }

            if (statement.superclass != null)
            {
                currentClass = ClassType.SUBCLASS;
                Resolve(statement.superclass);
                BeginScope();
                scopes.Peek()["super"] = true;
            }

            BeginScope();
            scopes.Peek()["this"] = true;

            foreach (Statement.Function method in statement.methods)
            {
                FunctionType declaration = FunctionType.METHOD;

                if (method.name.lexeme == "init")
                {
                    declaration = FunctionType.INITIALISER;
                }

                ResolveFunction(method, declaration);
            }

            EndScope();

            if (statement.superclass != null)
            {
                EndScope();
            }

            currentClass = enclosingClass;
            return null;
        }

        public object VisitBreakStatement(Statement.Break statement)
        {
            return null;
        }

        public object VisitCallExpression(Expression.Call expression)
        {
            Resolve(expression.callee);

            foreach (Expression argument in expression.arguments)
            {
                Resolve(argument);
            }
            return null;
        }

        public object VisitGetExpression(Expression.Get expression)
        {
            Resolve(expression.obj);
            return null;
        }

        public object VisitContinueStatement(Statement.Continue statement)
        {
            return null;
        }

        public object VisitExpressionStatement(Statement.Expression statement)
        {
            Resolve(statement.expression);
            return null;
        }

        public object VisitFunctionStatement(Statement.Function statement)
        {
            Declare(statement.name);
            Define(statement.name);

            ResolveFunction(statement, FunctionType.FUNCTION);
            return null;
        }

        public object VisitGroupingExpression(Expression.Grouping expression)
        {
            Resolve(expression.expression);
            return null;
        }

        public object VisitIfStatement(Statement.If statement)
        {
            Resolve(statement.condition);
            Resolve(statement.thenBranch);
            if (statement.elseBranch != null)
            {
                Resolve(statement.elseBranch);
            }
            return null;
        }

        public object VisitLiteralExpression(Expression.Literal expression)
        {
            return null;
        }

        public object VisitLogicalExpression(Expression.Logical expression)
        {
            Resolve(expression.left);
            Resolve(expression.right);
            return null;
        }

        public object VisitSetExpression(Expression.Set expression)
        {
            Resolve(expression.value);
            Resolve(expression.obj);
            return null;
        }
        public object VisitSuperExpression(Expression.Super expression)
        {
            if (currentClass == ClassType.NONE)
            {
                Program.Error(expression.keyword, "Can't use 'super' outside of a class.");
            } else if (currentClass != ClassType.SUBCLASS)
            {
                Program.Error(expression.keyword, "Can't use 'super' in a class with no superclass.");
            }
            ResolveLocal(expression, expression.keyword);
            return null;
        }
        
        public object VisitThisExpression(Expression.This expression)
        {
            if (currentClass == ClassType.NONE)
            {
                Program.Error(expression.keyword, "Can't use 'this' outside of a class.");
                return null;
            }

            ResolveLocal(expression, expression.keyword);
            return null;
        }

        public object VisitPrintStatement(Statement.Print statement)
        {
            Resolve(statement.expression);
            return null;
        }

        public object VisitReturnStatement(Statement.Return statement)
        {
            if (currentFunction == FunctionType.NONE)
            {
                Program.Error(statement.keyword, "Can't return from top-level code.");
            }
            if (statement.value != null)
            {
                if (currentFunction == FunctionType.INITIALISER)
                {
                    Program.Error(statement.keyword, "Can't return a value from an initializer.");
                }
                
                Resolve(statement.value);
            }
            return null;
        }

        public object VisitUnaryExpression(Expression.Unary expression)
        {
            Resolve(expression.right);
            return null;
        }

        public object VisitVariableExpression(Expression.Variable expression)
        {
            // We need to use try get since the Java get handles values that do not exist.
            if (scopes.Count > 0 && scopes.Peek().TryGetValue(expression.name.lexeme, out bool value))
            {
                if (!value)
                {
                    Program.Error(expression.name, "Can't read local variable in its own initializer.");
                }
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
            Resolve(statement.condition);
            Resolve(statement.body);
            return null;
        }
    }
}