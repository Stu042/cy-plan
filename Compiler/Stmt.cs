namespace Compiler;


public abstract class Stmt {
	public abstract void Accept(IStmtVisitor visitor);

	/// <summary>A group of statements. Usually starts with '{' and ends with '}' but also the global scope is a block.</summary>
	public class Block : Stmt {
		public required Stmt[] Statements;
		public override void Accept(IStmtVisitor visitor) {
			visitor.VisitBlockStmt(this);
		}
	}


	/// <summary>Variables defined as part of a function (input)</summary>
	public class InputVar : Stmt {
		public required Token Token;
		public required StmtType Type;

		public override void Accept(IStmtVisitor visitor) {
			visitor.VisitInputVarStmt(this);
		}
	}

	/// <summary>Function definition.</summary>
	public class Function : Stmt {// todo add function name with input args in name, i.e. Main_int_str[]
		public required Token FunctionNameToken;
		public required StmtType ReturnType;
		public required InputVar[] Input;
		public required Block Body;
		public override void Accept(IStmtVisitor visitor) {
			visitor.VisitFunctionStmt(this);
		}
	}


	public class For : Stmt {
		public required Token ForKeyword;
		public required StmtType IteratorType;
		public required Token Iterator;
		public required Expr Condition;
		public required Block Body;

		public For(Token forKeyword, StmtType iteratorType, Token iterator, Expr condition, Block body) {
			ForKeyword = forKeyword;
			IteratorType = iteratorType;
			Iterator = iterator;
			Condition = condition;
			Body = body;
		}
		public override void Accept(IStmtVisitor visitor) {
			visitor.VisitForStmt(this);
		}
	}

	public class If : Stmt {
		public required Token IfToken;
		public required Expr Condition;
		public required Block Body;
		public required Block ElseBody;

		public If(Token ifKeyword, Expr condition, Block body, Block elseBody) {
			IfToken = ifKeyword;
			Condition = condition;
			Body = body;
			ElseBody = elseBody;
		}
		public override void Accept(IStmtVisitor visitor) {
			visitor.VisitIfStmt(this);
		}
		public override string ToString() {
			return $"{IfToken.Lexeme} {Condition} THEN {Body} ELSE {ElseBody}";
		}
	}

	/// <summary>The return statement.</summary>
	public class Return : Stmt {
		public Expr? Value;
		public required Token ReturnToken;
		public override void Accept(IStmtVisitor visitor) {
			visitor.VisitReturnStmt(this);
		}
	}


	/// <summary>Variable declaration, with possible assignment.</summary>
	public class VarDefinition : Stmt {
		public required StmtType VarType;
		public required Token VarToken;
		public Expr? Initializer;
		public override void Accept(IStmtVisitor visitor) {
			visitor.VisitVarStmt(this);
		}
	}


	public class ClassDefinition : Stmt {
		public required Token ClassToken;
		public required VarDefinition[] Members;
		public required Function[] Methods;
		public required ClassDefinition[] Classes;

		public ClassDefinition(Token classToken, VarDefinition[] members, Function[] methods, ClassDefinition[] classes) {
			ClassToken = classToken;
			Members = members;
			Methods = methods;
			Classes = classes;
		}
		public override void Accept(IStmtVisitor visitor) {
			visitor.VisitClassStmt(this);
		}
	}

	/// <summary>A basic or user defined type.</summary>
	public class StmtType : Stmt {
		public required Token TypeToken;
		public override void Accept(IStmtVisitor visitor) {
			visitor.VisitTypeStmt(this);
		}
	}

	public class While : Stmt {
		public required Token WhileToken;
		public required Expr Condition;
		public required Block Body;

		public While(Token whileKeyword, Expr condition, Block body) {
			WhileToken = whileKeyword;
			Condition = condition;
			Body = body;
		}
		public override void Accept(IStmtVisitor visitor) {
			visitor.VisitWhileStmt(this);
		}
	}
}
