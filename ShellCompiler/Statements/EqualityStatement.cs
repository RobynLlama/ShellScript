using ShellCompiler.Blocks;

namespace ShellCompiler.Statements;

public class EqualityStatement(RunnableBlock leftHand, RunnableBlock rightHand) : IConditional
{
  private readonly RunnableBlock LeftHand = leftHand;
  private readonly RunnableBlock RightHand = rightHand;

  public bool Evaluate(ShellExecutable shell)
  {
    //Console.WriteLine($"Evaluating {LeftHand.GetParsedValue(shell)} == {RightHand.GetParsedValue(shell)}");
    return LeftHand.GetParsedValue(shell) == RightHand.GetParsedValue(shell);
  }
}
