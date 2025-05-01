using System.Text;


namespace Compiler;



public class TokeniserCursor {
	private const int StartLineOffset = 1;
	private const int StartLineNumber = 1;

	public string FileName { get; private set; }
	public int FilePosition { get; private set; }
	public int LineOffset { get; private set; }
	public int LineNumber { get; private set; }

	private string _contents;

	public TokeniserCursor(FileInfo fileInfo) {
		var file = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
		var buffer = new byte[file.Length];
		file.ReadExactly(buffer);
		file.Close();
		_contents = Encoding.ASCII.GetString(buffer);
		FileName = fileInfo.FullName;
		FilePosition = 0;
		LineOffset = StartLineOffset;
		LineNumber = StartLineNumber;
	}

	public char Current() {
		return FilePosition >= _contents.Length ? '\0' : _contents[FilePosition];
	}
	public char Next() {
		if (FilePosition >= _contents.Length) {
			return '\0';
		}
		if (_contents[FilePosition] == '\n') {
			LineNumber++;
			LineOffset = StartLineOffset - 1;
		}
		FilePosition++;
		LineOffset++;
		if (FilePosition >= _contents.Length) {
			return '\0';
		}
		while (_contents[FilePosition] == '\r') {
			FilePosition++;
		}
		return _contents[FilePosition];
	}
	public char PeekNext() {
		var filePosition = FilePosition;
		var lineNumber = LineNumber;
		var lineOffset = LineOffset;
		var ch = Next();
		FilePosition = filePosition;
		LineNumber = lineNumber;
		LineOffset = lineOffset;
		return ch;
	}

	public bool IsEndOfFile() {
		return FilePosition >= _contents.Length;
	}

	// Return substring from start pos up to current pos.
	public string SubString(int startPos) {
		var text = _contents.Substring(startPos, FilePosition - startPos + 1);
		return text;
	}
}
