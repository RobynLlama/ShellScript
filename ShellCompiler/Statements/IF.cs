using ShellCompiler.Blocks;

namespace ShellCompiler.Statements;

public class IFStatement(uint depth, IConditional condition, Statement[] run) : Statement
{
  public readonly uint Depth = depth;
  public readonly IConditional Condition = condition;
  public readonly Statement[] Run = run;

  public override void Execute(ShellExecutable shell)
  {
    if (Condition.Evaluate(shell))
    {
      foreach (var item in Run)
      {
        item.Execute(shell);
      }
    }
  }
}
