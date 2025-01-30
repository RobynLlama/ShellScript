using ShellCompiler.Blocks;

namespace ShellCompiler.Statements;

public class RunBinary(Literal binaryName, Literal[] arguments) : Statement
{
  public readonly Literal BinaryName = binaryName;
  public readonly Literal[] Arguments = arguments;

  public override void Execute(ShellExecutable shell)
  {
    shell.RunWithoutReturn(BinaryName.GetParsedValue(shell), [.. Arguments.Select(x => x.GetParsedValue(shell))]);
  }
}
