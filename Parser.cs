using System;
using System.Collections.Generic;

namespace LoxLangInCSharp
{
    public class Parser
    {
        private class ParseError : Exception { }

        private readonly List<Token> tokens;
        private int current = 0;
        private int loopDepth = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public List<Statement> Parse()
        {
            List<Statement> statements = new List<Statement>();

            while (!IsAtEnd())
            {
                statements.Add(Declaration());
            }

            return statements;
        }

        private Expression Expression()
        {
            return Assignment();
        }

        private Statement Declaration()
        {
            try
            {
                if (Match(TokenType.CLASS)) return ClassDeclaration();
                if (Match(TokenType.FUN)) return Function("function");
                if (Match(TokenType.VAR)) return VarDeclaration();

                return Statement();
            }
            catch (ParseError)
            {
                Synchronise();
                return null;
            }
        }

        private Statement ClassDeclaration()
        {
            Token name = Consume(TokenType.IDENTIFIER, "Expect class name.");
            Consume(TokenType.LEFT_BRACE, "Expect '{' before class body.");

            List<Statement.Function> methods = new List<Statement.Function>();
            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                methods.Add(Function("method"));
            }

            Consume(TokenType.RIGHT_BRACE, "Expect '}' after class body.");

            return new Statement.Class(name, methods);
        }

        private Statement Statement()
        {
            if (Match(TokenType.FOR)) return ForStatement();
            if (Match(TokenType.IF)) return IfStatement();
            if (Match(TokenType.PRINT)) return PrintStatement();
            if (Match(TokenType.RETURN)) return ReturnStatement();
            if (Match(TokenType.WHILE)) return WhileStatement();
            if (Match(TokenType.LEFT_BRACE)) return new Statement.Block(Block());
            if (Match(TokenType.BREAK)) return BreakStatement();
            if (Match(TokenType.CONTINUE)) return ContinueStatement();

            return ExpressionStatement();
        }

        private Statement ForStatement()
        {
            try
            {
                loopDepth++;

                Consume(TokenType.LEFT_PAREN, "Expect '(' after 'for'.");

                Statement initialiser = null;

                if (Match(TokenType.SEMICOLON))
                {

                }
                else if (Match(TokenType.VAR))
                {
                    initialiser = VarDeclaration();
                }
                else
                {
                    initialiser = ExpressionStatement();
                }

                Expression condition = Expression();

                if (!Check(TokenType.SEMICOLON))
                {
                    condition = Expression();
                }

                Consume(TokenType.SEMICOLON, "Expect ';' after loop condition.");

                Expression increment = null;

                if (!Check(TokenType.RIGHT_PAREN))
                {
                    increment = Expression();
                }

                Consume(TokenType.RIGHT_PAREN, "Expect ')' after for clauses.");

                Statement body = Statement();

                if (increment != null)
                {
                    body = new Statement.Block(new List<Statement> { body, new Statement.Expression(increment) });
                }

                if (condition == null) condition = new Expression.Literal(true);
                body = new Statement.While(condition, body);

                if (initialiser != null)
                {
                    body = new Statement.Block(new List<Statement> { initialiser, body });
                }

                return body;
            }
            finally
            {
                loopDepth--;
            }
        }

        private Statement IfStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expected '(' before expression.");
            Expression condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expected ')' after expression.");

            Statement thenBranch = Statement();
            Statement elseBranch = null;

            if (Match(TokenType.ELSE))
            {
                elseBranch = Statement();
            }

            return new Statement.If(condition, thenBranch, elseBranch);
        }

        private Statement PrintStatement()
        {
            Expression value = Expression();
            Consume(TokenType.SEMICOLON, "Expected ';' after value");
            return new Statement.Print(value);
        }

        private Statement ReturnStatement()
        {
            Token keyword = Previous();
            Expression value = null;

            if (!Check(TokenType.SEMICOLON))
            {
                value = Expression();
            }

            Consume(TokenType.SEMICOLON, "Expected ';' after value");
            return new Statement.Return(keyword, value);
        }

        private Statement VarDeclaration()
        {
            Token name = Consume(TokenType.IDENTIFIER, "Expect variable name.");
            Expression initialiser = null;

            if (Match(TokenType.EQUAL))
            {
                initialiser = Expression();
            }

            Consume(TokenType.SEMICOLON, "Expect ';' after variable declaration.");

            return new Statement.Var(name, initialiser);
        }

        private Statement WhileStatement()
        {
            try
            {
                loopDepth++;
                Consume(TokenType.LEFT_PAREN, "Expected '(' after 'while'.");
                Expression condition = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expected ')' after condition.");
                Statement body = Statement();

                return new Statement.While(condition, body);
            }
            finally
            {
                loopDepth--;
            }
        }

        private Statement BreakStatement()
        {
            if (loopDepth <= 0)
            {
                Error(Previous(), "Can only use 'break' inside a loop.");
            }
            Consume(TokenType.SEMICOLON, "Expected ';' after 'break'.");
            return new Statement.Break();
        }

        private Statement ContinueStatement()
        {
            if (loopDepth <= 0)
            {
                Error(Previous(), "Can only use 'continue' inside a loop.");
            }
            Consume(TokenType.SEMICOLON, "Expected ';' after 'continue'.");
            return new Statement.Continue();
        }

        private Statement ExpressionStatement()
        {
            Expression value = Expression();
            Consume(TokenType.SEMICOLON, "Expected ';' after expression.");
            return new Statement.Expression(value);
        }

        private Statement.Function Function(string kind)
        {
            Token name = Consume(TokenType.IDENTIFIER, $"Expect {kind} name.");
            Consume(TokenType.LEFT_PAREN, $"Expect '(' after {kind} name.");

            List<Token> parameters = new List<Token>();

            if (!Check(TokenType.RIGHT_PAREN))
            {
                do {
                    if (parameters.Count >= 255) {
                        Error(Peek(), "Can't have more than 255 parameters.");
                    }

                    parameters.Add(Consume(TokenType.IDENTIFIER, "Expect parameter name."));
                } while (Match(TokenType.COMMA));
            }

            Consume(TokenType.RIGHT_PAREN, "Expect ')' after parameters.");
            Consume(TokenType.LEFT_BRACE, $"Expect '{{' before '{kind}' body.");

            List<Statement> body = Block();

            return new Statement.Function(name, parameters, body);
        }

        private List<Statement> Block()
        {
            List<Statement> statements = new List<Statement>();

            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                statements.Add(Declaration());
            }

            Consume(TokenType.RIGHT_BRACE, "Expect '}' after block.");

            return statements;
        }

        private Expression Assignment()
        {
            Expression expression = Or();

            if (Match(TokenType.EQUAL))
            {
                Token equals = Previous();
                Expression value = Assignment();

                if (expression.GetType() == typeof(Expression.Variable))
                {
                    Token name = (expression as Expression.Variable).name;
                    return new Expression.Assign(name, value);
                }
                else if (expression.GetType() == typeof(Expression.Get))
                {
                    Expression.Get get = (Expression.Get)expression;
                    return new Expression.Set(get.obj, get.name, value);
                }

                Error(equals, "Invalid assignment target.");
            }

            return expression;
        }

        private Expression Or()
        {
            Expression expression = And();

            while (Match(TokenType.OR))
            {
                Token op = Previous();
                Expression right = And();
                expression = new Expression.Logical(expression, op, right);
            }

            return expression;
        }

        private Expression And()
        {
            Expression expression = Equality();

            while (Match(TokenType.AND))
            {
                Token op = Previous();
                Expression right = Equality();
                expression = new Expression.Logical(expression, op, right);
            }

            return expression;
        }

        private Expression Equality()
        {
            Expression left = Comparison();

            while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
            {
                Token op = Previous();
                Expression right = Comparison();
                left = new Expression.Binary(left, op, right);
            }

            return left;
        }

        private Expression Comparison()
        {
            Expression left = Term();


            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token op = Previous();
                Expression right = Term();
                left = new Expression.Binary(left, op, right);
            }

            return left;
        }

        private Expression Term()
        {
            Expression left = Factor();


            while (Match(TokenType.MINUS, TokenType.PLUS))
            {
                Token op = Previous();
                Expression right = Factor();
                left = new Expression.Binary(left, op, right);
            }

            return left;
        }

        private Expression Factor()
        {
            Expression left = Unary();


            while (Match(TokenType.SLASH, TokenType.STAR))
            {
                Token op = Previous();
                Expression right = Unary();
                left = new Expression.Binary(left, op, right);
            }

            return left;
        }

        private Expression Unary()
        {
            while (Match(TokenType.BANG, TokenType.MINUS))
            {
                Token op = Previous();
                Expression right = Unary();
                return new Expression.Unary(op, right);
            }

            return Call();
        }

        private Expression Call()
        {
            Expression expression = Primary();

            while (true)
            {
                if (Match(TokenType.LEFT_PAREN))
                {
                    expression = FinishCall(expression);
                }
                else if (Match(TokenType.DOT))
                {
                    Token name = Consume(TokenType.IDENTIFIER, "Expect property name after '.'.");
                    expression = new Expression.Get(expression, name);
                }
                else
                {
                    break;
                }
            }

            return expression;
        }

        private Expression FinishCall(Expression callee)
        {
            List<Expression> arguments = new List<Expression>();

            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    if (arguments.Count >= 255)
                    {
                        Error(Peek(), "Can't have more than 255 arguments.");
                    }
                    arguments.Add(Expression());
                }
                while (Match(TokenType.COMMA));
            }

            Token parenthesis = Consume(TokenType.RIGHT_PAREN, "Expected ')' after arguments.");

            return new Expression.Call(callee, parenthesis, arguments);
        }

        private Expression Primary()
        {
            if (Match(TokenType.FALSE)) return new Expression.Literal(false);
            if (Match(TokenType.TRUE)) return new Expression.Literal(true);
            if (Match(TokenType.NIL)) return new Expression.Literal(null);

            if (Match(TokenType.NUMBER, TokenType.STRING))
            {
                return new Expression.Literal(Previous().literal);
            }

            if (Match(TokenType.THIS))
            {
                return new Expression.This(Previous());
            }

            if (Match(TokenType.IDENTIFIER))
            {
                return new Expression.Variable(Previous());
            }

            if (Match(TokenType.LEFT_PAREN))
            {
                Expression expression = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");

                return new Expression.Grouping(expression);
            }

            throw Error(Peek(), "Expected expression.");
        }

        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();
            throw Error(Peek(), message);
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().type == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) current++;
            return Previous();
        }

        private bool IsAtEnd()
        {
            return Peek().type == TokenType.EOF;
        }

        private Token Peek()
        {
            return tokens[current];
        }

        private Token Previous()
        {
            return tokens[current - 1];
        }

        private ParseError Error(Token token, string message)
        {
            ReportError(token, message);
            return new ParseError();
        }

        private static void ReportError(Token token, string message)
        {
            if (token.type == TokenType.EOF)
            {
                Report(token.line, " at end", message);
            }
            else
            {
                Report(token.line, " at '" + token.lexeme + "'", message);
            }
        }

        private static void Report(int line, string where, string message)
        {
            Console.Error.WriteLine("[line " + line + "] Error" + where + ": " + message);
        }

        private void Synchronise()
        {
            Advance();

            while (!IsAtEnd())
            {
                if (Previous().type == TokenType.SEMICOLON) return;

                switch (Peek().type)
                {
                    case TokenType.CLASS:
                    case TokenType.FUN:
                    case TokenType.VAR:
                    case TokenType.FOR:
                    case TokenType.IF:
                    case TokenType.WHILE:
                    case TokenType.PRINT:
                    case TokenType.RETURN:
                        return;
                }

                Advance();
            }
        }

    }
}