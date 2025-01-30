using ShellCompiler.Blocks;

namespace ShellCompiler;

public static partial class Synthesizers
{
  public static Literal AssembleReadVariable(Queue<Block> blocks)
  {
    //Variable needs
    // literal for name

    if (blocks.Count == 0)
      throw new InvalidOperationException("Unexpected end of file while assembling a read variable");

    var next = blocks.Dequeue();

    if (next is not Literal lit)
      throw new InvalidOperationException($"Literal expected while assembling a read variable. Block: {next.GetType().Name}");

    lit.IsVariableLiteral = true;

    return lit;
  }
}
