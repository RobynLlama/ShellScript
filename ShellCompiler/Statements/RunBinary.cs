using ShellCompiler.Blocks;

namespace ShellCompiler.Statements;

public class RunBinary(RunnableBlock binaryName, RunnableBlock[] arguments) : Statement
{
  public readonly RunnableBlock BinaryName = binaryName;
  public readonly RunnableBlock[] Arguments = arguments;

  public override void Execute(ShellExecutable shell)
  {
    shell.RunWithoutReturn(BinaryName.GetParsedValue(shell), [.. Arguments.Select(x => x.GetParsedValue(shell))]);
  }
}
