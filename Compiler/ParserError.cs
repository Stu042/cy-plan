using System.Text;
using Microsoft.Extensions.Primitives;


namespace Compiler;


public static class ParserError {
	public static void Show(string message, ParserCursor cursor) {
		var msg = Message(message, cursor);
		Console.WriteLine(msg);
		cursor.MoveToNextLine();
	}
	public static string Message(string message, ParserCursor cursor) {
		var bob = new StringBuilder();
		cursor.PushPosition();
		cursor.MoveToStartOfLine();
		var src = cursor.LineText();
		cursor.PopPosition();
		var count = cursor.LineCharCount();
		bob.AppendLine(message + $" {cursor.Peek()}");
		bob.AppendLine(src);
		bob.Append(new string('-', count));
		bob.AppendLine("^");
		return bob.ToString();
	}
}
