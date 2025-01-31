using org.matheval;
using ShellCompiler.Blocks;

namespace ShellCompiler.Statements;

public class IFStatement(uint depth, ExpressionWrapper condition, Statement[] run) : Statement
{
  public readonly uint Depth = depth;
  public readonly ExpressionWrapper Condition = condition;
  public readonly Statement[] Run = run;

  public override void Execute(ShellExecutable shell)
  {
    if (Condition.RunAsConditional(shell))
    {
      foreach (var item in Run)
      {
        item.Execute(shell);
      }
    }
  }
}
