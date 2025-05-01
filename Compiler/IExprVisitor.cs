namespace Compiler;



public interface IExprVisitor {
	/// <summary>Deal with brackets, i.e. (x+y).</summary>
	public void VisitGroupingExpr(Expr.Grouping expr);

	/// <summary>Get a value from Expr to assign to a variable. i.e. a=b</summary>
	public void VisitAssignExpr(Expr.Assign expr);

	/// <summary>Multiply, add, subtract, etc...</summary>
	public void VisitBinaryExpr(Expr.Binary expr);

	/// <summary>Call a method/function.</summary>
	public void VisitCallExpr(Expr.Call expr);

	/// <summary>a.b</summary>
	public void VisitGetExpr(Expr.Get obj);

	/// <summary>A hardcoded value, i.e. "hello world" or 42</summary>
	public void VisitLiteralExpr(Expr.Literal expr);

	public void VisitSetExpr(Expr.Set expr);

	/// <summary>Minus and not (!)</summary>
	public void VisitUnaryExpr(Expr.Unary expr);

	public void VisitLogicalExpr(Expr.Logical expr);

	/// <summary>Simply a variable.</summary>
	public void VisitVariableExpr(Expr.Variable var);
}
