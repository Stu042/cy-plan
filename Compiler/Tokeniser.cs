namespace Compiler;



public class TokenisedFile {
	public required string FileName;
	public required Token[] Tokens;

	public void PrintTokens() {
		Console.WriteLine($"Tokens, File: {FileName}:");
		foreach (var token in Tokens) {
			Console.WriteLine($"\t{token}");
		}
	}
}


public class Tokeniser {
	private readonly TokeniserCursor? _cursor;
	private readonly FileInfo _fileInfo;
	public Tokeniser(FileInfo fileInfo) {
		_fileInfo = fileInfo;
		try {
			_cursor = new TokeniserCursor(fileInfo);
		} catch (Exception e) {
			Console.WriteLine(e.Message);
		}
	}

	public TokenisedFile? ScanFile() {
		if (_cursor == null) {
			return null;
		}
		var tokens = new List<Token>();
		var filename = RelativePath();
		var ch = _cursor.Current();
		while (true) {
			switch (ch) {
				case '\0':
					tokens.Add(new Token {
						Type = TokenType.Eof,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					return new TokenisedFile {
						FileName = filename,
						Tokens = tokens.ToArray()
					};
				case '\r':
					break;
				case '\n':
					tokens.Add(new Token {
						Type = TokenType.NewLine,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					break;
				case ' ':
					tokens.Add(new Token {
						Type = TokenType.Space,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					break;
				case '\t':
					tokens.Add(new Token {
						Type = TokenType.Tab,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					break;
				case '(':
					tokens.Add(new Token {
						Type = TokenType.LeftParen,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					break;
				case ')':
					tokens.Add(new Token {
						Type = TokenType.RightParen,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					break;
				case '{':
					tokens.Add(new Token {
						Type = TokenType.LeftBrace,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					break;
				case '}':
					tokens.Add(new Token {
						Type = TokenType.RightBrace,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					break;
				case ',':
					tokens.Add(new Token {
						Type = TokenType.Comma,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					break;
				case '.':
					tokens.Add(new Token {
						Type = TokenType.Dot,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					break;
				case '-':
					if (_cursor.PeekNext() == '-') {
						tokens.Add(new Token {
							Type = TokenType.MinusMinus,
							Lexeme = "--",
							LineNumber = _cursor.LineNumber,
							LineOffset = _cursor.LineOffset,
							FileName = filename
						});
						_cursor.Next();
					} else {
						tokens.Add(new Token {
							Type = TokenType.Minus,
							Lexeme = ch.ToString(),
							LineNumber = _cursor.LineNumber,
							LineOffset = _cursor.LineOffset,
							FileName = filename
						});
					}
					break;
				case '+':
					if (_cursor.PeekNext() == '+') {
						tokens.Add(new Token {
							Type = TokenType.PlusPlus,
							Lexeme = "++",
							LineNumber = _cursor.LineNumber,
							LineOffset = _cursor.LineOffset,
							FileName = filename
						});
						_cursor.Next();
					} else {
						tokens.Add(new Token {
							Type = TokenType.Plus,
							Lexeme = ch.ToString(),
							LineNumber = _cursor.LineNumber,
							LineOffset = _cursor.LineOffset,
							FileName = filename
						});
					}
					break;
				case ';':
					tokens.Add(new Token {
						Type = TokenType.Semicolon,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					break;
				case '/':
					if (_cursor.PeekNext() == '/') {
						var start = _cursor.FilePosition;
						var lineNumber = _cursor.LineNumber;
						var lineOffset = _cursor.LineOffset;
						char nxt;
						do {
							_cursor.Next();
							nxt = _cursor.PeekNext();
						} while (nxt != '\n' && nxt != '\r' && nxt != '\0');
						var lex = _cursor.SubString(start);
						tokens.Add(new Token {
							Type = TokenType.Remark,
							Lexeme = lex,
							LineNumber = lineNumber,
							LineOffset = lineOffset,
							FileName = filename
						});
					} else {
						tokens.Add(new Token {
							Type = TokenType.Slash,
							Lexeme = ch.ToString(),
							LineNumber = _cursor.LineNumber,
							LineOffset = _cursor.LineOffset,
							FileName = filename
						});
					}
					break;
				case '\\':
					tokens.Add(new Token {
						Type = TokenType.BackSlash,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					break;
				case '*':
					tokens.Add(new Token {
						Type = TokenType.Star,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					break;
				case ':':
					tokens.Add(new Token {
						Type = TokenType.Colon,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					break;
				case '#':
					tokens.Add(new Token {
						Type = TokenType.Hash,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					break;
				case '!':
					if (_cursor.PeekNext() == '=') {
						tokens.Add(new Token {
							Type = TokenType.BangEqual,
							Lexeme = "!=",
							LineNumber = _cursor.LineNumber,
							LineOffset = _cursor.LineOffset,
							FileName = filename
						});
						_cursor.Next();
					} else {
						tokens.Add(new Token {
							Type = TokenType.Bang,
							Lexeme = ch.ToString(),
							LineNumber = _cursor.LineNumber,
							LineOffset = _cursor.LineOffset,
							FileName = filename
						});
					}
					break;
				case '=':
					if (_cursor.PeekNext() == '=') {
						tokens.Add(new Token {
							Type = TokenType.EqualEqual,
							Lexeme = "==",
							LineNumber = _cursor.LineNumber,
							LineOffset = _cursor.LineOffset,
							FileName = filename
						});
						_cursor.Next();
					} else {
						tokens.Add(new Token {
							Type = TokenType.Equal,
							Lexeme = ch.ToString(),
							LineNumber = _cursor.LineNumber,
							LineOffset = _cursor.LineOffset,
							FileName = filename
						});
					}
					break;
				case '>':
					if (_cursor.PeekNext() == '=') {
						tokens.Add(new Token {
							Type = TokenType.GreaterEqual,
							Lexeme = ">=",
							LineNumber = _cursor.LineNumber,
							LineOffset = _cursor.LineOffset,
							FileName = filename
						});
						_cursor.Next();
					} else {
						tokens.Add(new Token {
							Type = TokenType.Greater,
							Lexeme = ch.ToString(),
							LineNumber = _cursor.LineNumber,
							LineOffset = _cursor.LineOffset,
							FileName = filename
						});
					}
					break;
				case '<':
					if (_cursor.PeekNext() == '=') {
						tokens.Add(new Token {
							Type = TokenType.LessEqual,
							Lexeme = "<=",
							LineNumber = _cursor.LineNumber,
							LineOffset = _cursor.LineOffset,
							FileName = filename
						});
						_cursor.Next();
					} else {
						tokens.Add(new Token {
							Type = TokenType.Less,
							Lexeme = ch.ToString(),
							LineNumber = _cursor.LineNumber,
							LineOffset = _cursor.LineOffset,
							FileName = filename
						});
					}
					break;
				case '&':
					tokens.Add(new Token {
						Type = TokenType.And,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					break;
				case '|':
					tokens.Add(new Token {
						Type = TokenType.Or,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					break;
				case '^':
					tokens.Add(new Token {
						Type = TokenType.Xor,
						Lexeme = ch.ToString(),
						LineNumber = _cursor.LineNumber,
						LineOffset = _cursor.LineOffset,
						FileName = filename
					});
					break;
				case '"':
					StrLiteral(tokens, filename);
					break;
				case >= '0' and <= '9':
					Number(tokens, filename);
					break;
				case >= 'A' and <= 'Z' or >= 'a' and <= 'z' or '_':
					AlNumeric(tokens, filename);
					break;
				default:
					Console.WriteLine($"Unexpected character '{ch}' at file {_fileInfo.Name}, position {_cursor.LineNumber}:{_cursor.LineOffset}");
					break;
			}
			ch = _cursor.Next();
		}
	}

	// Doesn't deal with text with embedded quotation marks
	private void StrLiteral(List<Token> tokens, string filename) {
		if (_cursor == null) {
			return;
		}
		var startPos = _cursor.FilePosition;
		var lineNumber = _cursor.LineNumber;
		var lineOffset = _cursor.LineOffset;
		var ch = _cursor.PeekNext();
		while (ch != '"') {
			_cursor.Next();
			ch = _cursor.PeekNext();
		}
		tokens.Add(new Token {
			Type = TokenType.StrLiteral,
			Lexeme = _cursor.SubString(startPos),
			LineNumber = lineNumber,
			LineOffset = lineOffset,
			FileName = filename
		});
	}


	private static class StatementNames {
		public const string This = "this";
		public const string If = "if";
		public const string Else = "else";
		public const string For = "for";
		public const string Each = "each";
		public const string While = "while";
		public const string Return = "return";
		public const string False = "false";
		public const string True = "true";
		public const string Null = "null";
		public const string Mut = "mut";
		public const string Transient = "transient";
		public const string Singleton = "singleton";
		public const string Config = "config";
		public const string Model = "model";
		public const string Class = "class";
		public const string New = "new";
		public const string Delete = "delete";
		public const string Defer = "defer";
	}


	private static class BasicTypeNames {
		public const string Int = "int";
		public const string Int8 = "i8";
		public const string Int16 = "i16";
		public const string Int32 = "i32";
		public const string Int64 = "i64";
		public const string UInt = "uint";
		public const string UInt8 = "u8";
		public const string UInt16 = "u16";
		public const string UInt32 = "u32";
		public const string UInt64 = "u64";
		public const string Float = "float";
		public const string Float16 = "f16";
		public const string Float32 = "f32";
		public const string Float64 = "f64";
		public const string Ascii = "str";
		public const string Utf8 = "utf8";
		public const string Bool = "bool";
		public const string Void = "void";
	}


	private static readonly Dictionary<string, TokenType> Keywords = new() {
		{ StatementNames.This, TokenType.This },
		{ StatementNames.If, TokenType.If },
		{ StatementNames.While, TokenType.While },
		{ StatementNames.For, TokenType.For },
		{ StatementNames.Each, TokenType.Each },
		{ StatementNames.Else, TokenType.Else },
		{ StatementNames.Return, TokenType.Return },
		{ StatementNames.False, TokenType.False },
		{ StatementNames.True, TokenType.True },
		{ StatementNames.Null, TokenType.Null },
		{ StatementNames.Mut, TokenType.Mut },
		{ StatementNames.Transient, TokenType.Transient },
		{ StatementNames.Singleton, TokenType.Singleton },
		{ StatementNames.Config, TokenType.Config },
		{ StatementNames.Model, TokenType.Model },
		{ StatementNames.Class, TokenType.Class },
		{ StatementNames.New, TokenType.New },
		{ StatementNames.Delete, TokenType.Delete },
		{ StatementNames.Defer, TokenType.Defer },
	};


	private static readonly Dictionary<string, TokenType> BaseTypes = new() {
		{ BasicTypeNames.Int, TokenType.Int },
		{ BasicTypeNames.Int8, TokenType.Int8 },
		{ BasicTypeNames.Int16, TokenType.Int16 },
		{ BasicTypeNames.Int32, TokenType.Int32 },
		{ BasicTypeNames.Int64, TokenType.Int64 },
		{ BasicTypeNames.UInt, TokenType.UInt },
		{ BasicTypeNames.UInt8, TokenType.UInt8 },
		{ BasicTypeNames.UInt16, TokenType.UInt16 },
		{ BasicTypeNames.UInt32, TokenType.UInt32 },
		{ BasicTypeNames.UInt64, TokenType.UInt64 },
		{ BasicTypeNames.Float, TokenType.Float },
		{ BasicTypeNames.Float16, TokenType.Float16 },
		{ BasicTypeNames.Float32, TokenType.Float32 },
		{ BasicTypeNames.Float64, TokenType.Float64 },
		{ BasicTypeNames.Ascii, TokenType.Ascii },
		{ BasicTypeNames.Utf8, TokenType.Utf8 },
		{ BasicTypeNames.Bool, TokenType.Bool },
		{ BasicTypeNames.Void, TokenType.Void },
	};


	// An identifier or keyword is likely, deal with it.
	private void AlNumeric(List<Token> tokens, string filename) {
		if (_cursor == null) {
			return;
		}
		var startPos = _cursor.FilePosition;
		var lineNumber = _cursor.LineNumber;
		var lineOffset = _cursor.LineOffset;
		var ch = _cursor.PeekNext();
		while (ch == '_' || char.IsLetter(ch) || char.IsDigit(ch)) {
			_cursor.Next();
			ch = _cursor.PeekNext();
		}
		var text = _cursor.SubString(startPos);
		var tokenType = TokenType.Identifier;
		if (Keywords.TryGetValue(text, out var keyword)) {
			tokenType = keyword;
		} else if (BaseTypes.TryGetValue(text, out var baseType)) {
			tokenType = baseType;
		}
		tokens.Add(new Token {
			Type = tokenType,
			Lexeme = text,
			LineNumber = lineNumber,
			LineOffset = lineOffset,
			FileName = filename
		});
	}


	// Only reads standard ints and floats, no scientific notation or hex, etc.
	private void Number(List<Token> tokens, string filename) {
		if (_cursor == null) {
			return;
		}
		var startPos = _cursor.FilePosition;
		var lineNumber = _cursor.LineNumber;
		var lineOffset = _cursor.LineOffset;
		var ch = _cursor.PeekNext();
		while (char.IsDigit(ch)) {
			_cursor.Next();
			ch = _cursor.PeekNext();
		}
		var tokenType = TokenType.IntLiteral;
		if (ch == '.') {
			ch = _cursor.Next();
			tokenType = TokenType.FloatLiteral;
			while (char.IsDigit(ch)) {
				_cursor.Next();
				ch = _cursor.PeekNext();
			}
		}
		tokens.Add(new Token {
			Type = tokenType,
			Lexeme = _cursor.SubString(startPos),
			LineNumber = lineNumber,
			LineOffset = lineOffset,
			FileName = filename
		});
	}

	private string RelativePath() {
		var dest = new Uri(_fileInfo.FullName);
		var here = new Uri(System.Environment.CurrentDirectory);
		var relativePath = here.MakeRelativeUri(dest).ToString();
		return relativePath;
	}
}
