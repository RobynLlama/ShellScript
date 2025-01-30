using ShellCompiler.Blocks;

namespace ShellCompiler.Statements;

public class EqualityStatement(Literal leftHand, Literal rightHand) : IConditional
{
  private readonly Literal LeftHand = leftHand;
  private readonly Literal RightHand = rightHand;

  public bool Evaluate(ShellExecutable shell)
  {
    Console.WriteLine($"Evaluating {LeftHand.Value} == {RightHand.Value}");
    return true;
  }
}
