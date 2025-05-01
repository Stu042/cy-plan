namespace Compiler;


public class AstMap : IAstVisitor{
	public AstMap() {

	}

	public void VisitBlockStmt(Stmt.Block stmt) {
		foreach (var statement in stmt.Statements) {
			statement.Accept(this);
		}
	}

	public void VisitClassStmt(Stmt.ClassDefinition stmt) {
		foreach (var classDefinition in stmt.Classes) {
			classDefinition.Accept(this);
		}
		foreach (var memberDef in stmt.Members) {
			memberDef.Accept(this);
		}
		foreach (var method in stmt.Methods) {
			method.Accept(this);
		}
	}

	public void VisitFunctionStmt(Stmt.Function stmt) {
		stmt.ReturnType.Accept(this);
		foreach (var param in stmt.Input) {
			param.Accept(this);
		}
		stmt.Body.Accept(this);
	}

	public void VisitInputVarStmt(Stmt.InputVar invar) {
		invar.Type.Accept(this);
	}
	public void VisitIfStmt(Stmt.If stmt) {
		stmt.Body.Accept(this);
		stmt.ElseBody.Accept(this);
	}
	public void VisitReturnStmt(Stmt.Return stmt) {
		stmt.Value?.Accept(this);
	}
	public void VisitTypeStmt(Stmt.StmtType stmt) { }

	public void VisitVarStmt(Stmt.VarDefinition stmt) {
		stmt.VarType.Accept(this);
		stmt.Initializer?.Accept(this);
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
		expr.Value.Accept(this);
	}

	public void VisitBinaryExpr(Expr.Binary expr) {
		expr.Left.Accept(this);
		expr.Right.Accept(this);
	}

	public void VisitCallExpr(Expr.Call expr) {
		expr.Callee.Accept(this);
		foreach (var arg in expr.Arguments) {
			arg.Accept(this);
		}
	}

	public void VisitGetExpr(Expr.Get obj) {
		throw new NotImplementedException();
	}

	public void VisitLiteralExpr(Expr.Literal expr) { }

	public void VisitSetExpr(Expr.Set expr) {
		throw new NotImplementedException();
	}
	public void VisitUnaryExpr(Expr.Unary expr) {
		throw new NotImplementedException();
	}
	public void VisitLogicalExpr(Expr.Logical expr) {
		throw new NotImplementedException();
	}
	public void VisitVariableExpr(Expr.Variable var) { }

}
