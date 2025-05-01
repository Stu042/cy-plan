namespace Compiler;


public class ParsedFileModel {
	public required TokenisedFile TokenisedFile;
	public required string FileName;
	public required Stmt.Block Statements;
	public void PrintStatements() {
		Console.WriteLine($"AST, File: {FileName}");
		var astPrinter = new AstString();
		astPrinter.VisitBlockStmt(Statements);
		Console.WriteLine(astPrinter.Text());
	}
}


public class Parser {
	private readonly TokenisedFile _tokenisedFile;
	private readonly Token[] _tokens;
	private readonly string _filename;
	private readonly ParserCursor _cursor;
	public Parser(TokenisedFile tokenisedFile) {
		_tokenisedFile = tokenisedFile;
		_tokens = tokenisedFile.Tokens;
		_filename = tokenisedFile.FileName;
		_cursor = new ParserCursor(tokenisedFile);
	}

	public ParsedFileModel Parse() {
		var block = Block();
		return new ParsedFileModel {
			TokenisedFile = _tokenisedFile,
			Statements = block,
			FileName = _filename,
		};
	}

	/// <summary>Get the current Block of statements. Doesn't consume braces but expects RightBrace or end of file for block end.</summary>
	private Stmt.Block Block() {
		var statements = new List<Stmt>();
		_cursor.SkipAllSpace();
		while (!_cursor.IsAtEnd() && !_cursor.QCheck(TokenType.RightBrace)) {
			try {
				var stmt = Declaration();
				statements.Add(stmt);
			} catch (Exception e) {
				Console.WriteLine(e);
				_cursor.MoveToNextLine();
			}
			_cursor.SkipAllSpace();
		}
		return new Stmt.Block {
			Statements = statements.ToArray()
		};
	}


	private Stmt Declaration() {
		_cursor.SkipAllSpace();
		if (_cursor.CheckStart().CheckAnyType().CheckSkipAllSpace(TokenType.Identifier).CheckSkipAllSpace(TokenType.LeftParen).CheckEnd()) {
			return DefineFunction();
		}
		if (_cursor.CheckStart().CheckAnyType().CheckSkipAllSpace(TokenType.Identifier).CheckEnd()) {
			return VarDeclaration();
		}
		return Statement();
	}

	/// <summary>Create a variable, which optionally has an assigned expression.</summary>
	private Stmt.VarDefinition VarDeclaration() {
		_cursor.SkipAllSpace();
		if (!_cursor.CheckStart().CheckBasicType().CheckEnd() && !_cursor.QCheck(TokenType.Identifier)) {
			throw new ParserException(_cursor, "Expect variable type.");
		}
		var typeToken = _cursor.Next();
		_cursor.SkipSpace();
		var name = _cursor.Next();
		if (name.Type != TokenType.Identifier) {
			throw new ParserException(_cursor, "Expect variable name.");
		}
		Expr? initializer = null;
		_cursor.SkipSpace();
		if (_cursor.QCheck(TokenType.Equal)) {
			_ = _cursor.Next();	// grab the =
			initializer = Expression();
		}
		return new Stmt.VarDefinition {
			VarType = new Stmt.StmtType {
				TypeToken = typeToken
			},
			VarToken = name,
			Initializer = initializer
		};
	}

	/// <summary>Function definition, starts with function return type, name, param list and ends with block delimited with Left and Right Brace.
	/// Expects at least identifier, identifier, left paren or basic type, identifier, left paren at start (with spaces between).</summary>
	private Stmt.Function DefineFunction() {
		_cursor.SkipAllSpace();
		var returnTypeTok = _cursor.Next();
		var returnType = new Stmt.StmtType {
			TypeToken = returnTypeTok
		};
		_cursor.SkipAllSpace();
		var funcNameTok = _cursor.Next();
		_cursor.SkipAllSpace();
		_cursor.Consume(TokenType.LeftParen, $"Expected Left Parenthesis in function {funcNameTok.Lexeme}.");
		var parameters = GetParamList();
		_cursor.Consume(TokenType.RightParen, $"Expected Right Parenthesis in function {funcNameTok.Lexeme}.");
		_cursor.SkipAllSpace();
		_cursor.Consume(TokenType.LeftBrace, $"Expect Left Brace before code block in function {funcNameTok.Lexeme}.");
		var body = Block();
		_cursor.Consume(TokenType.RightBrace, $"Expect Right Brace after code block in function {funcNameTok.Lexeme}");
		return new Stmt.Function {
			FunctionNameToken = funcNameTok,
			Body = body,
			Input = parameters,
			ReturnType = returnType,
		};
	}

	/// <summary>Parse a statement, i.e. For, If, Return, etc.</summary>
	private Stmt Statement() {
		var tokenType = _cursor.Peek().Type;
		return tokenType switch {
			//TokenType.For => ForStatement(),
			//TokenType.If => IfStatement(),
			TokenType.Return => ReturnStatement(),
			//TokenType.While => WhileStatement(),
			//_ => ExpressionStatement(),
		};
	}

	private Stmt.Return ReturnStatement() {
		var keyword = _cursor.Next();
		var value = Expression();
		return new Stmt.Return {
			ReturnToken = keyword,
			Value = value
		};
	}

	/// <summary>Create an expression. i.e. a+b or 2+3 or rhs of an assign, a=2, etc...</summary>
	private Expr Expression() {
		_cursor.SkipSpace();
		var expr = Assignment();
		return expr;
	}

	private Expr Assignment() {
		var expr = Or();
		_cursor.SkipSpace();
		if (!_cursor.QCheck(TokenType.Equal)) {
			return expr;
		}
		_ = _cursor.Next();
		_cursor.SkipSpace();
		var value = Assignment();
		switch (expr) {
			case Expr.Variable exprVar: {
				var name = exprVar.Token;
				return new Expr.Assign {
					Name = name,
					Value = value
				};
			}
			case Expr.Get exprGet:
				return new Expr.Set {
					Obj = exprGet.Obj,
					Token = exprGet.Token,
					Value = value
				};
			default:
				throw new ParserException(_cursor, "Invalid assignment target.");
		}
	}

	private Expr Or() {
		var expr = And();
		_cursor.SkipSpace();
		while (_cursor.QCheck(TokenType.Or)) {
			var op = _cursor.Next();
			_cursor.SkipSpace();
			var right = And();
			expr = new Expr.Logical {
				Left = expr,
				Operator = op,
				Right = right
			};
		}
		return expr;
	}

	private Expr And() {
		var expr = Equality();
		_cursor.SkipSpace();
		while (_cursor.QCheck(TokenType.And)) {
			var op = _cursor.Next();
			_cursor.SkipSpace();
			var right = Equality();
			expr = new Expr.Logical {
				Left = expr,
				Operator = op,
				Right = right,
			};
		}
		return expr;
	}

	/// <summary>Expression tests for not equal and equal.</summary>
	private Expr Equality() {
		var expr = Compare();
		_cursor.SkipSpace();
		while (_cursor.CheckStart().CheckAny(TokenType.BangEqual, TokenType.EqualEqual).CheckEnd()) {
			var op = _cursor.Next();
			var right = Compare();
			expr = new Expr.Binary {
				Left = expr,
				OpToken = op,
				Right = right
			};
		}
		return expr;
	}

	private Expr Compare() {
		var expr = Addition();
		_cursor.SkipSpace();
		while (_cursor.CheckStart().CheckAny(TokenType.Greater, TokenType.GreaterEqual, TokenType.Less, TokenType.LessEqual).CheckEnd()) {
			var op = _cursor.Next();
			var right = Addition();
			expr = new Expr.Binary {
				Left = expr,
				OpToken = op,
				Right = right
			};
		}
		return expr;
	}

	/// <summary>Expression is an addition or subtraction.</summary>
	private Expr Addition() {
		var expr = Multiplication();
		_cursor.SkipSpace();
		while (_cursor.CheckStart().CheckAny(TokenType.Minus, TokenType.Plus).CheckEnd()) {
			var op = _cursor.Next();
			var right = Multiplication();
			expr = new Expr.Binary {
				Left = expr,
				OpToken = op,
				Right = right
			};

		}
		return expr;
	}

	/// <summary>Expression is a multiplication or division.</summary>
	private Expr Multiplication() {
		var expr = Unary();
		_cursor.SkipSpace();
		while (_cursor.CheckStart().CheckAny(TokenType.Slash, TokenType.Star).CheckEnd()) {
			var op = _cursor.Next();
			var right = Unary();
			expr = new Expr.Binary {
				Left = expr,
				OpToken = op,
				Right = right
			};

		}
		return expr;
	}

	/// <summary>Pre expression operator, i.e. !a, -a, --a, ++a, etc. Expression is a number.</summary>
	private Expr Unary() {
		_cursor.SkipSpace();
		if (_cursor.QCheckAny(TokenType.Bang, TokenType.Minus, TokenType.MinusMinus, TokenType.PlusPlus)) {
			var op = _cursor.Next();
			var right = Unary();
			return new Expr.Unary{
				Token = op,
				Right = right
			};
		}
		return Call();
	}

	/// <summary>Call a method/function.</summary>
	private Expr Call() {
		_cursor.SkipSpace();
		var expr = Primary();
		_cursor.SkipSpace();
		while (true) {
			if (_cursor.QCheck(TokenType.LeftParen)) {
				expr = FinishCall(expr);
			} else if (_cursor.QCheck(TokenType.Dot)) {
				var name = _cursor.Next();
				if (name.Type != TokenType.Identifier) {
					throw new ParserException(_cursor, "Expect property name after dot.");
				}
				expr = new Expr.Get {
					Obj = expr,
					Token = name
				};
			} else {
				break;
			}
		}
		return expr;
	}

	/// <summary>Parse an expression.</summary>
	private Expr Primary() {
		var startToken = _cursor.Peek();
		if (_cursor.QCheck(TokenType.False)) {
			return new Expr.Literal {
				Token = _cursor.Next(),
				Value = false
			};
		}
		if (_cursor.QCheck(TokenType.True)) {
			return new Expr.Literal {
				Token = _cursor.Next(),
				Value = true
			};
		}
		if (_cursor.QCheck(TokenType.Null)) {
			return new Expr.Literal {
				Token = _cursor.Next(),
				Value = null
			};
		}
		if (_cursor.QCheck(TokenType.Identifier)) {
			Expr expr = new Expr.Variable{
				Token = _cursor.Next()
			};
			while (_cursor.QCheck(TokenType.Dot)) {
				_ = _cursor.Next();
				var name = _cursor.Next();
				if (name.Type != TokenType.Identifier) {
					throw new ParserException(_cursor, "Expect property name after dot.");
				}
				expr = new Expr.Get {
					Obj = expr,
					Token = name
				};
			}
			return expr;
		}
		if (IsLiteral(_cursor.Peek().Type)) {
			var tok = _cursor.Next();
			return new Expr.Literal {
				Token = tok,
				Value = tok.Literal
			};
		}
		if (_cursor.QCheck(TokenType.LeftParen)) {
			var expr = Expression();
			_cursor.Consume(TokenType.RightParen, "Expect matching ')' after expression.");
			return new Expr.Grouping {
				Token = startToken,
				Expr = expr
			};
		}
		throw new ParserException(_cursor, "Expect property name after expression.");
	}

	private static bool IsLiteral(TokenType tokenType) {
		TokenType[] literals = [TokenType.StrLiteral, TokenType.IntLiteral, TokenType.FloatLiteral];
		return literals.Contains(tokenType);
	}

	/// <summary>Finish calling a method/function.</summary>
	private Expr.Call FinishCall(Expr callee) {
		var arguments = new List<Expr>();
		_cursor.Consume(TokenType.LeftParen, "Expect '(' after call.");
		while (!_cursor.QCheck(TokenType.RightParen)) {
			arguments.Add(Expression());
			_cursor.SkipAllSpace();
			if (_cursor.QCheck(TokenType.Comma)) {
				_ = _cursor.Next();	// grab comma
				_cursor.SkipAllSpace();
			}
		}
		_cursor.SkipAllSpace();
		var rightParen = _cursor.Consume(TokenType.RightParen, "Expect ')' after call.");
		return new Expr.Call {
			Callee = callee,
			Token = rightParen,
			Arguments = arguments.ToArray()
		};
	}


	/// <summary>Return array of params. Does not consume the delimiters, but expects to end with right parens.</summary>
	private Stmt.InputVar[] GetParamList() {
		var parameters = new List<Stmt.InputVar>();
		while (!_cursor.QCheck(TokenType.RightParen) && !_cursor.IsAtEnd()) {
			_cursor.SkipAllSpace();
			var typeTok = _cursor.Next();
			_cursor.SkipAllSpace();
			var identifierTok = _cursor.Next();
			var type = new Stmt.StmtType {
				TypeToken = typeTok
			};
			var param = new Stmt.InputVar {
				Token = identifierTok,
				Type = type,
			};
			parameters.Add(param);
			_cursor.SkipAllSpace();
			if (_cursor.Peek().Type == TokenType.Comma) {
				_cursor.Next();
			}
		}
		return parameters.ToArray();
	}
}
