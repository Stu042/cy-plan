using System.Text;
using Compiler;
using Xunit.Abstractions;
using FluentAssertions;


namespace TestCompiler;



public class UnitTestTokeniser {
	private readonly ITestOutputHelper _testOutputHelper;

	public UnitTestTokeniser(ITestOutputHelper testOutputHelper) {
		_testOutputHelper = testOutputHelper;
	}


	private const string TestNumbersCy = "0123456789";
	[Fact]
	public void TestNumbers_SimpleInt() {
		var fileInfo = WriteFile(TestNumbersCy);
		var tokeniser = new Tokeniser(fileInfo);
		var tokens = tokeniser.ScanFile();
		PrintTokens(System.Reflection.MethodBase.GetCurrentMethod()?.Name, tokens);
		var filename = fileInfo.FullName.Replace('\\', '/');
		tokens.Should().NotBeNull();
		tokens.Tokens
			.Should()
			.NotBeEmpty()
			.And.HaveCount(2)
			.And.BeEquivalentTo([
				new Token {
					LineNumber = 1,
					LineOffset = 1,
					FileName = filename,
					Type = TokenType.IntLiteral,
					Lexeme = "0123456789",
					Literal = null
				},
				new Token {
					LineNumber = 1,
					LineOffset = 11,
					FileName = filename,
					Type = TokenType.Eof,
					Lexeme = "\0",
					Literal = null
				}
			]);
	}


	private const string TestSimpleCy = """
										// Simple mult app
										int Mult(int a, int b) {
											return a * b
										}

										int Main() {
											int result = Mult(5, 4)
											return result
										}
										""";

	[Fact]
	public void Test_SimpleCy() {
		var fileInfo = WriteFile(TestSimpleCy);
		var tokeniser = new Tokeniser(fileInfo);
		var tokens = tokeniser.ScanFile();
		PrintTokens(System.Reflection.MethodBase.GetCurrentMethod()?.Name, tokens);
		var filename = fileInfo.FullName.Replace('\\', '/');
		tokens.Should().NotBeNull();
		tokens.Tokens
			.Should()
			.NotBeEmpty()
			.And.HaveCount(60)
			.And.BeEquivalentTo([
				new Token { FileName = filename, Lexeme = "// Simple mult app", Type = TokenType.Remark, LineNumber = 1, LineOffset = 1 },
				new Token { FileName = filename, Lexeme = "\n", Type = TokenType.NewLine, LineNumber = 1, LineOffset = 19 },
				new Token { FileName = filename, Lexeme = "int", Type = TokenType.Int, LineNumber = 2, LineOffset = 1 },
				new Token { FileName = filename, Lexeme = " ", Type = TokenType.Space, LineNumber = 2, LineOffset = 4 },
				new Token { FileName = filename, Lexeme = "Mult", Type = TokenType.Identifier, LineNumber = 2, LineOffset = 5 },
				new Token { FileName = filename, Lexeme = "(", Type = TokenType.LeftParen, LineNumber = 2, LineOffset = 9 },
				new Token { FileName = filename, Lexeme = "int", Type = TokenType.Int, LineNumber = 2, LineOffset = 10 },
				new Token { FileName = filename, Lexeme = " ", Type = TokenType.Space, LineNumber = 2, LineOffset = 13 },
				new Token { FileName = filename, Lexeme = "a", Type = TokenType.Identifier, LineNumber = 2, LineOffset = 14 },
				new Token { FileName = filename, Lexeme = ",", Type = TokenType.Comma, LineNumber = 2, LineOffset = 15 },
				new Token { FileName = filename, Lexeme = " ", Type = TokenType.Space, LineNumber = 2, LineOffset = 16 },
				new Token { FileName = filename, Lexeme = "int", Type = TokenType.Int, LineNumber = 2, LineOffset = 17 },
				new Token { FileName = filename, Lexeme = " ", Type = TokenType.Space, LineNumber = 2, LineOffset = 20 },
				new Token { FileName = filename, Lexeme = "b", Type = TokenType.Identifier, LineNumber = 2, LineOffset = 21 },
				new Token { FileName = filename, Lexeme = ")", Type = TokenType.RightParen, LineNumber = 2, LineOffset = 22 },
				new Token { FileName = filename, Lexeme = " ", Type = TokenType.Space, LineNumber = 2, LineOffset = 23 },
				new Token { FileName = filename, Lexeme = "{", Type = TokenType.LeftBrace, LineNumber = 2, LineOffset = 24 },
				new Token { FileName = filename, Lexeme = "\n", Type = TokenType.NewLine, LineNumber = 2, LineOffset = 25 },
				new Token { FileName = filename, Lexeme = "\t", Type = TokenType.Tab, LineNumber = 3, LineOffset = 1 },
				new Token { FileName = filename, Lexeme = "return", Type = TokenType.Return, LineNumber = 3, LineOffset = 2 },
				new Token { FileName = filename, Lexeme = " ", Type = TokenType.Space, LineNumber = 3, LineOffset = 8 },
				new Token { FileName = filename, Lexeme = "a", Type = TokenType.Identifier, LineNumber = 3, LineOffset = 9 },
				new Token { FileName = filename, Lexeme = " ", Type = TokenType.Space, LineNumber = 3, LineOffset = 10 },
				new Token { FileName = filename, Lexeme = "*", Type = TokenType.Star, LineNumber = 3, LineOffset = 11 },
				new Token { FileName = filename, Lexeme = " ", Type = TokenType.Space, LineNumber = 3, LineOffset = 12 },
				new Token { FileName = filename, Lexeme = "b", Type = TokenType.Identifier, LineNumber = 3, LineOffset = 13 },
				new Token { FileName = filename, Lexeme = "\n", Type = TokenType.NewLine, LineNumber = 3, LineOffset = 14 },
				new Token { FileName = filename, Lexeme = "}", Type = TokenType.RightBrace, LineNumber = 4, LineOffset = 1 },
				new Token { FileName = filename, Lexeme = "\n", Type = TokenType.NewLine, LineNumber = 4, LineOffset = 2 },
				new Token { FileName = filename, Lexeme = "\n", Type = TokenType.NewLine, LineNumber = 5, LineOffset = 1 },
				new Token { FileName = filename, Lexeme = "int", Type = TokenType.Int, LineNumber = 6, LineOffset = 1 },
				new Token { FileName = filename, Lexeme = " ", Type = TokenType.Space, LineNumber = 6, LineOffset = 4 },
				new Token { FileName = filename, Lexeme = "Main", Type = TokenType.Identifier, LineNumber = 6, LineOffset = 5 },
				new Token { FileName = filename, Lexeme = "(", Type = TokenType.LeftParen, LineNumber = 6, LineOffset = 9 },
				new Token { FileName = filename, Lexeme = ")", Type = TokenType.RightParen, LineNumber = 6, LineOffset = 10 },
				new Token { FileName = filename, Lexeme = " ", Type = TokenType.Space, LineNumber = 6, LineOffset = 11 },
				new Token { FileName = filename, Lexeme = "{", Type = TokenType.LeftBrace, LineNumber = 6, LineOffset = 12 },
				new Token { FileName = filename, Lexeme = "\n", Type = TokenType.NewLine, LineNumber = 6, LineOffset = 13 },
				new Token { FileName = filename, Lexeme = "\t", Type = TokenType.Tab, LineNumber = 7, LineOffset = 1 },
				new Token { FileName = filename, Lexeme = "int", Type = TokenType.Int, LineNumber = 7, LineOffset = 2 },
				new Token { FileName = filename, Lexeme = " ", Type = TokenType.Space, LineNumber = 7, LineOffset = 5 },
				new Token { FileName = filename, Lexeme = "result", Type = TokenType.Identifier, LineNumber = 7, LineOffset = 6 },
				new Token { FileName = filename, Lexeme = " ", Type = TokenType.Space, LineNumber = 7, LineOffset = 12 },
				new Token { FileName = filename, Lexeme = "=", Type = TokenType.Equal, LineNumber = 7, LineOffset = 13 },
				new Token { FileName = filename, Lexeme = " ", Type = TokenType.Space, LineNumber = 7, LineOffset = 14 },
				new Token { FileName = filename, Lexeme = "Mult", Type = TokenType.Identifier, LineNumber = 7, LineOffset = 15 },
				new Token { FileName = filename, Lexeme = "(", Type = TokenType.LeftParen, LineNumber = 7, LineOffset = 19 },
				new Token { FileName = filename, Lexeme = "5", Type = TokenType.IntLiteral, LineNumber = 7, LineOffset = 20 },
				new Token { FileName = filename, Lexeme = ",", Type = TokenType.Comma, LineNumber = 7, LineOffset = 21 },
				new Token { FileName = filename, Lexeme = " ", Type = TokenType.Space, LineNumber = 7, LineOffset = 22 },
				new Token { FileName = filename, Lexeme = "4", Type = TokenType.IntLiteral, LineNumber = 7, LineOffset = 23 },
				new Token { FileName = filename, Lexeme = ")", Type = TokenType.RightParen, LineNumber = 7, LineOffset = 24 },
				new Token { FileName = filename, Lexeme = "\n", Type = TokenType.NewLine, LineNumber = 7, LineOffset = 25 },
				new Token { FileName = filename, Lexeme = "\t", Type = TokenType.Tab, LineNumber = 8, LineOffset = 1 },
				new Token { FileName = filename, Lexeme = "return", Type = TokenType.Return, LineNumber = 8, LineOffset = 2 },
				new Token { FileName = filename, Lexeme = " ", Type = TokenType.Space, LineNumber = 8, LineOffset = 8 },
				new Token { FileName = filename, Lexeme = "result", Type = TokenType.Identifier, LineNumber = 8, LineOffset = 9 },
				new Token { FileName = filename, Lexeme = "\n", Type = TokenType.NewLine, LineNumber = 8, LineOffset = 15 },
				new Token { FileName = filename, Lexeme = "}", Type = TokenType.RightBrace, LineNumber = 9, LineOffset = 1 },
				new Token { FileName = filename, Lexeme = "\0", Type = TokenType.Eof, LineNumber = 9, LineOffset = 2 }
			]);
	}



	private void PrintTokens(string? testName, TokenisedFile? tokens) {
		_testOutputHelper.WriteLine($"{testName}, Tokens:");
		if (tokens == null) {
			_testOutputHelper.WriteLine("\tNo tokens, tokeniser returned null.");
			return;
		}
		foreach (var token in tokens.Tokens) {
			_testOutputHelper.WriteLine($"\t{token}");
		}
	}

	private static FileInfo WriteFile(string contents) {
		var path = Path.GetTempFileName();
		var fileInfo = new FileInfo(path);
		var fileStream = fileInfo.Create();
		var testNumbersBytes = Encoding.ASCII.GetBytes(contents);
		fileStream.Write(testNumbersBytes);
		fileStream.Close();
		return fileInfo;
	}
}
