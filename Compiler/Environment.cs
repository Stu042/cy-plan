namespace Compiler;




/// <summary>The full namespace environment, so includes all classes, variables functions etc.
/// DI as Singleton</summary>
public class Environment {
	public const string GlobalName = "Global";

	private readonly Env _root; // the global environment

	private Env _current;

	public Environment() {
		_root = new Env(GlobalName, null, EnvType.Global);
		_current = _root;
	}


	// ///////////////////////////
	// Build Environment functions

	/// <summary>Add a variable or property</summary>
	public void AddProperty(Token token) {
		var env = new Env(token.Lexeme, _current, EnvType.Property);
		_current.Children.Add(token.Lexeme, env);
	}

	/// <summary>End a AnonBlock, Class or Function</summary>
	public void End() {
		if (_current.Parent != null) {
			_current = _current.Parent;
		}
	}

	/// <summary>Start of a class</summary>
	public void StartClass(Token token) {
		Start(token.Lexeme, EnvType.Class);
	}
	/// <summary>Start of a function</summary>
	public void StartFunction(Token token) {
		Start(token.Lexeme, EnvType.Function);
	}
	/// <summary>Start of an anonymous block</summary>
	public void StartAnonBlock(Token token) {
		Start($"---{token.FileName}{token.LineNumber}", EnvType.AnonBlock);
	}

	// ///////////////////////////
	// Use Environment functions

	public void MoveToGlobal() {
		_current = _root;
	}

	/// <summary>Find from current scope</summary>
	public Env? Find(string instanceName) {
		var needle = Find(instanceName, _current);
		return needle;
	}

	/// <summary>Find from specified scope</summary>
	public Env? Find(string instanceName, Env env) {
		var haystack = env;
		while (haystack != null) {
			if (haystack.Children.TryGetValue(instanceName, out var needle)) {
				return needle;
			}
			haystack = haystack.Parent;
		}
		return null;
	}


	private void Start(string name, EnvType type) {
		var env = new Env(name, _current, type);
		_current.Children.Add(name, env);
		_current = env;
	}


	/// <summary>An enclosed environment, only includes vars etc that are with this block.</summary>
	public class Env {
		public readonly string InstanceName; // name of this env
		public readonly Env? Parent;
		public readonly EnvType Type; // type of this env, can be a global namespace, a class, etc...
		public readonly Dictionary<string, Env> Children; // children accessed by the identifier name

		public Env(string instanceName, Env? parent, EnvType type) {
			InstanceName = instanceName;
			Parent = parent;
			Type = type;
			Children = [];
		}
	}
}


public enum EnvType {
	Global,
	Class,
	Function,
	Property,
	AnonBlock
}
