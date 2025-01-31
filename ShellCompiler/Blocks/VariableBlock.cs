using ShellCompiler.Statements;

namespace ShellCompiler.Blocks;

public class VariableBlock(string input) : RunnableBlock
{
  public string Varname = input[1..];
  public override string RawValue { get => Varname; }
  public override string GetParsedValue(ShellExecutable shell) =>
    shell.GetVariable(Varname);

}
