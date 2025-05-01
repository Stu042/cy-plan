using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace cy_plan;



internal static class Program {
	internal static void Main(string[] args) {
		var serviceCollection = new ServiceCollection().AddLogging(builder => {
#if DEBUG
			builder.AddConsole();
			builder.SetMinimumLevel(LogLevel.Debug);
#else
			builder.SetMinimumLevel(LogLevel.Error);
#endif
		});
		serviceCollection.AddTransient<Compiler.Compiler>();
		Compiler.Init.Services(serviceCollection);
		var cmdLine = new CmdLine(serviceCollection);
		cmdLine.ParseCmdLine(args);
		var serviceProvider = serviceCollection.BuildServiceProvider();
		var app = serviceProvider.GetService<Compiler.Compiler>();
		if (app == null) {
			Console.WriteLine("App could not be found.");
			return;
		}
		app.Init();
		app.Run();
		app.Done();
	}
}
