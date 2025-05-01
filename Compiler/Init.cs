using Microsoft.Extensions.DependencyInjection;

namespace Compiler;


public static class Init {
	public static void Services(IServiceCollection serviceProvider) {
		serviceProvider
			.AddTransient<Compiler>()
			.AddTransient<CompileTaskRunner>()
			.AddSingleton<Environment>();
	}
}

