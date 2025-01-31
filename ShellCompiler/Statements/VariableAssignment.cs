using ShellCompiler.Blocks;

namespace ShellCompiler.Statements;

public class VariableAssignment(RunnableBlock variableName, RunnableBlock assignment) : Statement
{
  public readonly RunnableBlock VariableName = variableName;
  public readonly RunnableBlock Assignment = assignment;

  public override void Execute(ShellExecutable shell)
  {
    shell.SetVariable(VariableName.GetParsedValue(shell), Assignment.GetParsedValue(shell));
  }
}
