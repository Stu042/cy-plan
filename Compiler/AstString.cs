using System.Text;


namespace Compiler;



public class AstString : IAstVisitor {
	private StringBuilder _builder;

	public AstString() {
		_builder = new StringBuilder();
	}

	public string Text() {
		return _builder.ToString();
	}

	public void VisitBlockStmt(Stmt.Block stmt) {
		_builder.Append("(block ");
		foreach (var statement in stmt.Statements) {
			statement.Accept(this);
		}
		_builder.Append(')');
	}

	public void VisitClassStmt(Stmt.ClassDefinition stmt) {
		_builder.Append($"({stmt.ClassToken.Lexeme}: ");
		foreach (var classDefinition in stmt.Classes) {
			classDefinition.Accept(this);
		}
		foreach (var memberDef in stmt.Members) {
			memberDef.Accept(this);
		}
		foreach (var method in stmt.Methods) {
			method.Accept(this);
		}
		_builder.Append(')');
	}

	public void VisitFunctionStmt(Stmt.Function stmt) {
		_builder.Append('(');
		stmt.ReturnType.Accept(this);
		_builder.Append($" {stmt.FunctionNameToken.Lexeme}(");
		var count = stmt.Input.Length;
		foreach (var param in stmt.Input) {
			param.Accept(this);
			if (--count > 0) {
				_builder.Append(", ");
			}
		}
		_builder.Append(") (");
		stmt.Body.Accept(this);
		_builder.Append(')');
	}

	public void VisitInputVarStmt(Stmt.InputVar invar) {
		invar.Type.Accept(this);
		_builder.Append($" {invar.Token.Lexeme}");
	}
	public void VisitIfStmt(Stmt.If stmt) {
		var condition = Parenthesize("if", stmt.Condition);
		_builder.Append($"{condition} (");
		stmt.Body.Accept(this);
		_builder.Append(") else (");
		stmt.ElseBody.Accept(this);
		_builder.Append(')');
	}
	public void VisitReturnStmt(Stmt.Return stmt) {
		_builder.Append("(return ");
		stmt.Value?.Accept(this);
		_builder.Append(')');
	}
	public void VisitTypeStmt(Stmt.StmtType stmt) {
		_builder.Append(stmt.TypeToken.Lexeme);
	}

	public void VisitVarStmt(Stmt.VarDefinition stmt) {
		stmt.VarType.Accept(this);
		_builder.Append($" {stmt.VarToken.Lexeme}");
		if (stmt.Initializer != null) {
			_builder.Append(" = ");
			stmt.Initializer.Accept(this);
		}
	}

	public void VisitForStmt(Stmt.For stmt) {
		throw new NotImplementedException();
	}
	public void VisitWhileStmt(Stmt.While stmt) {
		throw new NotImplementedException();
	}
	public void VisitGroupingExpr(Expr.Grouping expr) {
		throw new NotImplementedException();
	}
	public void VisitAssignExpr(Expr.Assign expr) {
		_builder.Append($"{expr.Name.Lexeme} = ");
		expr.Value.Accept(this);
	}

	public void VisitBinaryExpr(Expr.Binary expr) {
		expr.Left.Accept(this);
		_builder.Append($" {expr.OpToken.Lexeme} ");
		expr.Right.Accept(this);
	}

	public void VisitCallExpr(Expr.Call expr) {
		_builder.Append("call ");	// {expr.Token.Lexeme}
		expr.Callee.Accept(this);
		var count = expr.Arguments.Length;
		_builder.Append('(');
		foreach (var arg in expr.Arguments) {
			arg.Accept(this);
			if (--count > 0) {
				_builder.Append(", ");
			}
		}
		_builder.Append(')');
	}

	public void VisitGetExpr(Expr.Get obj) {
		throw new NotImplementedException();
	}

	public void VisitLiteralExpr(Expr.Literal expr) {
		_builder.Append(expr.Token.Lexeme);
	}

	public void VisitSetExpr(Expr.Set expr) {
		throw new NotImplementedException();
	}
	public void VisitUnaryExpr(Expr.Unary expr) {
		throw new NotImplementedException();
	}
	public void VisitLogicalExpr(Expr.Logical expr) {
		throw new NotImplementedException();
	}
	public void VisitVariableExpr(Expr.Variable var) {
		_builder.Append(var.Token.Lexeme);
	}

	private string Parenthesize(string name, params Expr[] exprs) {
		var builder = new StringBuilder();
		builder.Append('(').Append(name);
		foreach (var expr in exprs) {
			builder.Append(' ');
			expr.Accept(this);
		}
		builder.Append(')');
		return builder.ToString();
	}
}
