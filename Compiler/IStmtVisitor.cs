namespace Compiler;



public interface IStmtVisitor {
	/// <summary>A group of statements, i.e. multiple lines of code.</summary>
	public void VisitBlockStmt(Stmt.Block stmt);
	/// <summary>A class (object), group of properties and methods.</summary>
	public void VisitClassStmt(Stmt.ClassDefinition stmt);
	/// <summary>A function.</summary>
	public void VisitFunctionStmt(Stmt.Function stmt);
	/// <summary>Input to a function.</summary>
	public void VisitInputVarStmt(Stmt.InputVar invar);
	/// <summary>An if statement.</summary>
	public void VisitIfStmt(Stmt.If stmt);
	/// <summary>A return statement.</summary>
	public void VisitReturnStmt(Stmt.Return stmt);
	/// <summary>A type for a variable or function statement.</summary>
	public void VisitTypeStmt(Stmt.StmtType stmt);
	/// <summary>A variable statement.</summary>
	public void VisitVarStmt(Stmt.VarDefinition stmt);
	/// <summary>A for loop statement.</summary>
	public void VisitForStmt(Stmt.For stmt);
	/// <summary>A while loop statement.</summary>
	public void VisitWhileStmt(Stmt.While stmt);
}
