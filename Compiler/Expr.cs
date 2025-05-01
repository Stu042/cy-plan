using System.Text;


namespace Compiler;



public abstract class Expr {
	public abstract void Accept(IExprVisitor visitor);

	/// <summary>Get a value from Expr to assign to a variable.</summary>
	public class Assign : Expr {
		public required Expr Value;// value to assign
		public required Token Name;// variables name

		public override void Accept(IExprVisitor visitor) {
			visitor.VisitAssignExpr(this);
		}
	}

	/// <summary>Like (x*a) </summary>
	public class Grouping : Expr {
		public required Expr Expr;
		public required Token Token;

		public override void Accept(IExprVisitor visitor) {
			visitor.VisitGroupingExpr(this);
		}
	}

	/// <summary>A hardcoded value, i.e. a number.</summary>
	public class Literal : Expr {
		public required object? Value;
		public required Token Token;
		public override void Accept(IExprVisitor visitor) {
			visitor.VisitLiteralExpr(this);
		}
	}


	/// <summary>Set a properties value.</summary>
	public class Set : Expr {
		public required Expr Obj;
		public required Token Token;
		public required Expr Value;
		public override void Accept(IExprVisitor visitor) {
			visitor.VisitSetExpr(this);
		}
	}


	/// <summary>Get a properties value.</summary>
	public class Get : Expr {
		public required Expr Obj;
		public required Token Token;
		public override void Accept(IExprVisitor visitor) {
			visitor.VisitGetExpr(this);
		}
	}


	/// <summary>Multiply, add, subtract, Left + Right, Left - Right, etc... </summary>
	public class Binary : Expr {
		public required Expr Left;
		public required Token OpToken;
		public required Expr Right;

		public override void Accept(IExprVisitor visitor) {
			visitor.VisitBinaryExpr(this);
		}
	}

	/// <summary>Call a method/function.</summary>
	public class Call : Expr {           // token is end of function call - rparen
		public required Expr Callee;     // function we are calling (might be a constructor with no function body as yet)
		public required Expr[] Arguments;// input args
		public required Token Token;	 // useless? equals closing parenthesis of function call

		public override void Accept(IExprVisitor visitor) {
			visitor.VisitCallExpr(this);
		}
	}

	/// <summary>Simply a variable.</summary>
	public class Variable : Expr {
		public required Token Token;

		public override void Accept(IExprVisitor visitor) {
			visitor.VisitVariableExpr(this);
		}
	}


	// todo add pre -- and ++
	/// <summary>Minus and not (!)</summary>
	public class Unary : Expr {
		public required Expr Right;
		public required Token Token;

		public override void Accept(IExprVisitor visitor) {
			visitor.VisitUnaryExpr(this);
		}
	}

	/// <summary>And, Or, Xor</summary>
	public class Logical : Expr {
		public required Expr Left;
		public required Token Operator;
		public required Expr Right;

		public override void Accept(IExprVisitor visitor) {
			visitor.VisitLogicalExpr(this);
		}
	}
}
