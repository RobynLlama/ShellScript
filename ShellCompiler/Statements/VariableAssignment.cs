using ShellCompiler.Blocks;

namespace ShellCompiler.Statements;

public class VariableAssignment(Literal variableName, Literal assignment) : Statement
{
  public readonly Literal VariableName = variableName;
  public readonly Literal Assignment = assignment;

  public override void Execute(ShellExecutable shell)
  {
    shell.SetVariable(VariableName.GetParsedValue(shell), Assignment.GetParsedValue(shell));
  }
}
