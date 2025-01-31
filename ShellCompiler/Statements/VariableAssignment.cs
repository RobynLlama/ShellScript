using ShellCompiler.Blocks;

namespace ShellCompiler.Statements;

public class VariableAssignment(RunnableBlock variableName, ExpressionWrapper assignment) : Statement
{
  public readonly RunnableBlock VariableName = variableName;
  public readonly ExpressionWrapper Assignment = assignment;

  public override void Execute(ShellExecutable shell)
  {
    shell.SetVariable(VariableName.GetParsedValue(shell), Assignment.Eval(shell));
  }
}
