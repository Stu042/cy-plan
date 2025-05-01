using System.CommandLine;
using System.CommandLine.Binding;
using Config;
using Microsoft.Extensions.DependencyInjection;


namespace cy_plan;



public class Person {
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
}


public class PersonBinder : BinderBase<Person> {
	private readonly Option<string> _firstNameOption;
	private readonly Option<string> _lastNameOption;

	public PersonBinder(Option<string> firstNameOption, Option<string> lastNameOption) {
		_firstNameOption = firstNameOption;
		_lastNameOption = lastNameOption;
	}

	protected override Person GetBoundValue(BindingContext bindingContext) =>
		new Person {
			FirstName = bindingContext.ParseResult.GetValueForOption(_firstNameOption),
			LastName = bindingContext.ParseResult.GetValueForOption(_lastNameOption)
		};
}


internal class CmdLine {
	private readonly IServiceCollection _serviceCollection;

	public CmdLine(IServiceCollection serviceCollection) {
		_serviceCollection = serviceCollection;
	}

	internal void ParseCmdLine(string[] args) {
		var fileOption = new Option<FileInfo?>(name: "--file", description: "An option whose argument is parsed as a FileInfo", getDefaultValue: () => new FileInfo("scl.runtimeconfig.json"));
		var firstNameOption = new Option<string>(name: "--first-name", description: "Person.FirstName");
		var lastNameOption = new Option<string>(name: "--last-name", description: "Person.LastName");
		var rootCommand = new RootCommand {
			fileOption,
			firstNameOption,
			lastNameOption
		};
		rootCommand.SetHandler(DoRootCommand, fileOption, new PersonBinder(firstNameOption, lastNameOption));

		var srcFileOption = new Option<FileInfo?>(name: "--src", description: "File to compile.");
		var compileCommand = new Command("compile", "Compile files.") {
			srcFileOption,
		};
		compileCommand.SetHandler(CompileCommand, srcFileOption);
		rootCommand.Add(compileCommand);
		rootCommand.InvokeAsync(args).Wait();
	}

	private void CompileCommand(FileInfo? srcFile) {

		var compileConfig = new CompileConfig {
			SrcFile = GetSrcFile(srcFile)
		};
		_serviceCollection.AddSingleton<CompileConfig>(compileConfig);
		Console.WriteLine($"Compiling {srcFile.FullName}");
	}

	private FileInfo GetSrcFile(FileInfo? srcFile) {
		if (srcFile == null) {
			Console.WriteLine($"Compiler missing src file.");
			Environment.Exit(-1);
		}
		if (!srcFile.Exists) {
			Console.WriteLine($"File {srcFile.FullName} does not exist.");
			Environment.Exit(-1);
		}
		return srcFile;
	}

	private void DoRootCommand(FileInfo? aFile, Person aPerson) {
		Console.WriteLine("Try adding as command");
	}
}
