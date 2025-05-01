namespace Compiler;


class ParserException : Exception {
	public ParserException(ParserCursor cursor, string message) : base(ParserError.Message(message, cursor)) {	}
	public override string ToString() {
		return Message;
	}
}
