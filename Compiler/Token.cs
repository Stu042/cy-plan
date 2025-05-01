namespace Compiler;



public class Token {
	public required TokenType Type;		// type of token
	public required string Lexeme;		// actual text of token from source
	public object? Literal;				// value if a literal
	public required int LineNumber;		// line number (starts at 1)
	public required int LineOffset;		// Position from start of line
	public required string FileName;	// source filename

	public override string ToString() {
		var lex = Lexeme switch {
			"\n" => "-NEWLINE-",
			"\0" => "-EOF-",
			"\t" => "-TAB-",
			" " => "-SPACE-",
			_ => Lexeme
		};
		var formattedStr = $"{FileName} {lex} {Type} {LineNumber}:{LineOffset}";
		return formattedStr;
	}

	public string ToSmallString() {
		var lex = Lexeme switch {
			"\n" => "-NEWLINE-",
			"\0" => "-EOF-",
			"\t" => "-TAB-",
			" " => "-SPACE-",
			_ => Lexeme
		};
		var formattedStr = $"{Type}:{lex} {LineNumber}:{LineOffset}";
		return formattedStr;
	}
}
