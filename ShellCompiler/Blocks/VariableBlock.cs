using ShellCompiler.Statements;

namespace ShellCompiler.Blocks;

public class VariableBlock(string input) : RunnableBlock
{
  public readonly string Varname = input;
  public override string RawValue { get => Varname; }
  public override string GetParsedValue(ShellExecutable shell) =>
    shell.GetVariable(Varname).GetValueForTerminal();

}
