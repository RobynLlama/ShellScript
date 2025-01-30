using ShellCompiler.Blocks;

namespace ShellCompiler.Statements;

public class RunBinary(Literal binaryName, Literal[] arguments) : Statement
{
  public readonly Literal BinaryName = binaryName;
  public readonly Literal[] Arguments = arguments;

  public override object? Execute(ShellExecutable shell)
  {
    return shell.RunWithReturn(BinaryName.GetParsedValue(shell), [.. Arguments.Select(x => x.GetParsedValue(shell))]);
  }
}
