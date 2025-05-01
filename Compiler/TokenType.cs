namespace Compiler;



public enum TokenType {
	// Single-character tokens.
	LeftParen, RightParen, LeftBrace, RightBrace,
	Comma, Dot, Minus, Plus, Semicolon, Slash, BackSlash,
	Star, Colon, Hash,

	// One or two character tokens.
	Bang, BangEqual, Equal, EqualEqual, Greater, GreaterEqual, Less, LessEqual,
	MinusMinus, PlusPlus,

	// types
	Int, Int8, Int16, Int32, Int64,
	UInt, UInt8, UInt16, UInt32, UInt64,
	Float, Float16, Float32, Float64,
	Ascii, Utf8, Bool, Void,

	Mut,

	Identifier,

	// Literals.
	StrLiteral, IntLiteral, FloatLiteral,


	// Keywords.
	And, Or, Xor, Else, For, Each, If, Null, Return, This, True, False, While,
	Transient, Singleton, Config, Model, Class, New, Delete, Defer,

	// not relevant to compilation
	Remark,

	Space, Tab, NewLine, Eof
}

