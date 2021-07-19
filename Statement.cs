using System;
using System.Collections.Generic;

namespace LoxLangInCSharp {
public abstract class Statement {
public interface IVisitor<T> {
public T VisitBlockStatement (Block statement);
public T VisitClassStatement (Class statement);
public T VisitBreakStatement (Break statement);
public T VisitContinueStatement (Continue statement);
public T VisitExpressionStatement (Expression statement);
public T VisitFunctionStatement (Function statement);
public T VisitIfStatement (If statement);
public T VisitPrintStatement (Print statement);
public T VisitReturnStatement (Return statement);
public T VisitVarStatement (Var statement);
public T VisitWhileStatement (While statement);
}
public class Block : Statement {
public Block (List<LoxLangInCSharp.Statement> statements) {
this.statements = statements;
}

public override T Accept<T> (IVisitor<T> visitor) {
return visitor.VisitBlockStatement(this);
}

public readonly List<LoxLangInCSharp.Statement> statements;
}
public class Class : Statement {
public Class (Token name, List<LoxLangInCSharp.Statement.Function> methods) {
this.name = name;
this.methods = methods;
}

public override T Accept<T> (IVisitor<T> visitor) {
return visitor.VisitClassStatement(this);
}

public readonly Token name;
public readonly List<LoxLangInCSharp.Statement.Function> methods;
}
public class Break : Statement {
public Break () {
}

public override T Accept<T> (IVisitor<T> visitor) {
return visitor.VisitBreakStatement(this);
}

}
public class Continue : Statement {
public Continue () {
}

public override T Accept<T> (IVisitor<T> visitor) {
return visitor.VisitContinueStatement(this);
}

}
public class Expression : Statement {
public Expression (LoxLangInCSharp.Expression expression) {
this.expression = expression;
}

public override T Accept<T> (IVisitor<T> visitor) {
return visitor.VisitExpressionStatement(this);
}

public readonly LoxLangInCSharp.Expression expression;
}
public class Function : Statement {
public Function (Token name, List<Token> parameters, List<LoxLangInCSharp.Statement> body) {
this.name = name;
this.parameters = parameters;
this.body = body;
}

public override T Accept<T> (IVisitor<T> visitor) {
return visitor.VisitFunctionStatement(this);
}

public readonly Token name;
public readonly List<Token> parameters;
public readonly List<LoxLangInCSharp.Statement> body;
}
public class If : Statement {
public If (LoxLangInCSharp.Expression condition, LoxLangInCSharp.Statement thenBranch, LoxLangInCSharp.Statement elseBranch) {
this.condition = condition;
this.thenBranch = thenBranch;
this.elseBranch = elseBranch;
}

public override T Accept<T> (IVisitor<T> visitor) {
return visitor.VisitIfStatement(this);
}

public readonly LoxLangInCSharp.Expression condition;
public readonly LoxLangInCSharp.Statement thenBranch;
public readonly LoxLangInCSharp.Statement elseBranch;
}
public class Print : Statement {
public Print (LoxLangInCSharp.Expression expression) {
this.expression = expression;
}

public override T Accept<T> (IVisitor<T> visitor) {
return visitor.VisitPrintStatement(this);
}

public readonly LoxLangInCSharp.Expression expression;
}
public class Return : Statement {
public Return (Token keyword, LoxLangInCSharp.Expression value) {
this.keyword = keyword;
this.value = value;
}

public override T Accept<T> (IVisitor<T> visitor) {
return visitor.VisitReturnStatement(this);
}

public readonly Token keyword;
public readonly LoxLangInCSharp.Expression value;
}
public class Var : Statement {
public Var (Token name, LoxLangInCSharp.Expression initialiser) {
this.name = name;
this.initialiser = initialiser;
}

public override T Accept<T> (IVisitor<T> visitor) {
return visitor.VisitVarStatement(this);
}

public readonly Token name;
public readonly LoxLangInCSharp.Expression initialiser;
}
public class While : Statement {
public While (LoxLangInCSharp.Expression condition, LoxLangInCSharp.Statement body) {
this.condition = condition;
this.body = body;
}

public override T Accept<T> (IVisitor<T> visitor) {
return visitor.VisitWhileStatement(this);
}

public readonly LoxLangInCSharp.Expression condition;
public readonly LoxLangInCSharp.Statement body;
}

public abstract T Accept<T> (IVisitor<T> visitor);
}
}
