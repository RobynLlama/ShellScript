using ShellCompiler.Blocks;

namespace ShellCompiler;

public static partial class Synthesizers
{
  public static Literal AssembleReadVariable(Queue<IToken> tokens)
  {
    //Variable needs
    // literal for name

    if (tokens.Count == 0)
      throw new InvalidOperationException("Unexpected end of file while assembling a read variable");

    var next = tokens.Dequeue();

    if (next is not Literal lit)
      throw new InvalidOperationException($"Literal expected while assembling a read variable. Block: {next.GetType().Name}");

    lit.IsVariableLiteral = true;

    return lit;
  }
}
