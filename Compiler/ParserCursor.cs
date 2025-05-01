using System.Text;


namespace Compiler;


public class ParserCursor {
	private readonly Token[] _tokens;
	public readonly string FileName;
	private readonly Stack<int> _posStack;
	private int _pos;
	private int _offset;
	private bool _checkResult;

	private readonly TokenType[] _space = [TokenType.Space, TokenType.Tab, TokenType.Remark];
	private readonly TokenType[] _allSpace = [TokenType.Space, TokenType.Tab, TokenType.NewLine, TokenType.Remark];

	private readonly TokenType[] _basicTypes = [
		TokenType.Float,
		TokenType.Int,
		TokenType.Bool,
		TokenType.Ascii,
		TokenType.Utf8,
		TokenType.Int8,
		TokenType.Int16,
		TokenType.Int32,
		TokenType.Int64,
		TokenType.UInt8,
		TokenType.UInt16,
		TokenType.UInt32,
		TokenType.UInt64,
		TokenType.Float32,
		TokenType.Float64,
	];
	private readonly TokenType[] _allTypes = [
		TokenType.Float,
		TokenType.Int,
		TokenType.Bool,
		TokenType.Ascii,
		TokenType.Utf8,
		TokenType.Int8,
		TokenType.Int16,
		TokenType.Int32,
		TokenType.Int64,
		TokenType.UInt8,
		TokenType.UInt16,
		TokenType.UInt32,
		TokenType.UInt64,
		TokenType.Float32,
		TokenType.Float64,
		TokenType.Identifier
	];

	public ParserCursor(TokenisedFile tokenisedFile) {
		_tokens = tokenisedFile.Tokens;
		FileName = tokenisedFile.FileName;
		_posStack = [];
		_pos = 0;
		_offset = 0;
		_checkResult = true;
	}


	// ////////
	// Move pos - peek, get, etc at cursor position.

	/// <summary>Look at current token.</summary>
	public Token Peek() {
		return IsAtEnd() ? _tokens[^1] : _tokens[_pos];
	}
	/// <summary>Return current token and move to next token.</summary>
	public Token Next() {
		return _tokens[_pos++];
	}
	/// <summary>Are we at the end of the tokens/file?</summary>
	public bool IsAtEnd(int offset = 0) {
		return _pos + offset >= _tokens.Length || _tokens[_pos + offset].Type == TokenType.Eof;
	}
	/// <summary>Move cursor to start of current line.</summary>
	public void MoveToStartOfLine() {
		while (_pos > 0 && _tokens[_pos - 1].Type != TokenType.NewLine) {
			--_pos;
		}
	}



	// ////
	// Skip - ignore specified tokens.

	public void SkipAny(params TokenType[] allToSkip) {
		while (allToSkip.Contains(_tokens[_pos].Type) && !IsAtEnd()) {
			_pos++;
		}
	}

	public void SkipSpace() {
		while (_space.Contains(_tokens[_pos].Type)) {
			_pos++;
		}
	}

	public void SkipAllSpace() {
		while (_allSpace.Contains(_tokens[_pos].Type)) {
			_pos++;
		}
	}


	// ////////
	// Consumes - expect a token, if found return it else return null and show error.
	public Token Consume(TokenType tokenType, string errorMessage) {
		if (_tokens[_pos].Type == tokenType) {
			return _tokens[_pos++];
		}
		ParserError.Show(errorMessage, this);
		throw new ParserException(this, errorMessage);
	}

	public void PushPosition() {
		_posStack.Push(_pos);
	}
	public void PopPosition() {
		_pos = _posStack.Pop();
	}


	// /////
	// Check - check, without moving cursor, for expected tokens.


	/// <summary>Checks current token is expected. Doesn't use offset.</summary>
	public bool QCheck(TokenType expected) {
		return _tokens[_pos].Type == expected;
	}

	/// <summary>Checks current token is any of the expected. Doesn't use offset.</summary>
	public bool QCheckAny(params TokenType[] any) {
		return any.Contains(_tokens[_pos].Type);
	}

	public ParserCursor CheckStart(int startOffset = 0) {
		_offset = startOffset;
		_checkResult = true;
		return this;
	}

	public bool CheckEnd() {
		return _checkResult;
	}

	public ParserCursor Check(TokenType expected) {
		if (_checkResult && _tokens[_pos + _offset].Type == expected) {
			_offset++;
		} else {
			_checkResult = false;
		}
		return this;
	}
	public ParserCursor CheckAll(params TokenType[] all) {
		if (!_checkResult) {
			return this;
		}
		foreach (var a in all) {
			if (_tokens[_pos + _offset].Type != a) {
				_checkResult = false;
				return this;
			}
		}
		return this;
	}
	public ParserCursor CheckSkip(TokenType expected, params TokenType[] skip) {
		if (!_checkResult) {
			return this;
		}
		while (skip.Contains(_tokens[_pos + _offset].Type)) {
			_offset++;
		}
		if (_tokens[_pos + _offset].Type == expected) {
			_offset++;
		} else {
			_checkResult = false;
		}
		return this;
	}
	public ParserCursor CheckSkipAllSpace(TokenType expected) {
		return CheckSkip(expected, _allSpace);
	}

	public ParserCursor CheckAny(params TokenType[] any) {
		if (_checkResult && any.Contains(_tokens[_pos + _offset].Type)) {
			_offset++;
		} else {
			_checkResult = false;
		}
		return this;
	}
	public ParserCursor CheckAnySkip(TokenType[] any, params TokenType[] skip) {
		if (!_checkResult) {
			return this;
		}
		while (skip.Contains(_tokens[_pos + _offset].Type)) {
			_offset++;
		}
		if (any.Contains(_tokens[_pos + _offset].Type)) {
			_offset++;
		} else {
			_checkResult = false;
		}
		return this;
	}
	public ParserCursor CheckAnySkipAllSpace(params TokenType[] any) {
		return CheckAnySkip(any, _allSpace);
	}
	public ParserCursor CheckBasicType() {
		if (_checkResult && _basicTypes.Contains(_tokens[_pos + _offset].Type)) {
			_offset++;
		} else {
			_checkResult = false;
		}
		return this;
	}

	/// <summary>Check next token is a type declaration or an identifier which could be a type declaration.</summary>
	public ParserCursor CheckAnyType() {
		return CheckAny(_allTypes);
	}

	/// <summary>Check next token is a type declaration or an identifier which could be a type declaration.</summary>
	public ParserCursor CheckAllTypesSkipAllSpace() {
		return CheckAnySkipAllSpace(_allTypes);
	}


	// ////
	// Text functions - return text, mostly for error reporting.


	/// <summary>Return text from current position to end of line.</summary>
	public string LineText() {
		var bob = new StringBuilder();
		PushPosition();
		while (_pos < _tokens.Length && _tokens[_pos].Type != TokenType.NewLine) {
			bob.Append(_tokens[_pos].Type == TokenType.Tab ? "    " : _tokens[_pos].Lexeme);
			_pos++;
		}
		PopPosition();
		return bob.ToString();
	}

	/// <summary>Return length of chars up to current position on this line.</summary>
	public int LineCharCount() {
		var startPos = _pos;
		PushPosition();
		MoveToStartOfLine();
		var count = 0;
		while (_pos < startPos) {
			count += _tokens[_pos].Type == TokenType.Tab ? 4 : _tokens[_pos].Lexeme.Length;
			_pos++;
		}
		PopPosition();
		return count;
	}
	public void MoveToNextLine() {
		while (_tokens[_pos++].Type != TokenType.NewLine && _pos < _tokens.Length) { }
	}
}
