using Config;
using Microsoft.Extensions.Logging;


namespace Compiler;



public class Compiler {
	private readonly CompileTaskRunner _compileTaskRunner;
	private readonly CompileConfig _compileConfig;
	private readonly ILogger<Compiler> _logger;

	public Compiler(CompileTaskRunner compileTaskRunner, CompileConfig compileConfig, ILogger<Compiler> logger) {
		_compileTaskRunner = compileTaskRunner;
		_compileConfig = compileConfig;
		_logger = logger;
	}

	public void Init() {
	}

	public void Run() {
		var firstFileId = _compileTaskRunner.ScanFile(_compileConfig.SrcFile);
		var first = _compileTaskRunner.Result(firstFileId);
		if (first == null) {
			Console.WriteLine($"Unable to compile first file, {_compileConfig.SrcFile.Name}");
			return;
		}
		first.TokenisedFile.PrintTokens();
		Console.WriteLine();
		first.PrintStatements();
	}

	public void Done() {
	}
}


public class CompileTaskRunner {
	private readonly List<CompileTaskModel> _taskList;
	private readonly ILogger<CompileTaskRunner> _logger;
	private int _currentId;

	public CompileTaskRunner(ILogger<CompileTaskRunner> logger) {
		_taskList = [];
		_logger = logger;
		_currentId = 0;
	}

	public int ScanFile(FileInfo fileInfo) {
		var task = Task.Factory.StartNew(() => Parse(fileInfo));
		_taskList.Add(new CompileTaskModel {
			Id = _currentId,
			Statements = task
		});
		return _currentId++;
	}

	// Run as an async task from ScanFile()
	private static ParsedFileModel Parse(FileInfo fileInfo) {
		var tokeniser = new Tokeniser(fileInfo);
		var tokens = tokeniser.ScanFile();
		if (tokens == null) {
			return new ParsedFileModel {
				TokenisedFile = new TokenisedFile {
					Tokens = [],
					FileName = fileInfo.Name
				},
				FileName = fileInfo.Name,
				Statements = new Stmt.Block {
					Statements = []
				}
			};
		}
		var parser = new Parser(tokens);
		var statements = parser.Parse();
		return statements;
	}

	public ParsedFileModel? Result(int id) {
		var task = _taskList.FirstOrDefault(tl => tl.Id == id);
		if (task == null) {
			return null;
		}
		_taskList.Remove(task);
		return task.Statements.Result;
	}

	public bool? IsCompleted(int id) {
		var task = _taskList.FirstOrDefault(tl => tl.Id == id);
		return task?.Statements.IsCompleted;
	}


	private class CompileTaskModel {
		public required int Id;
		public required Task<ParsedFileModel> Statements;
	}
}
