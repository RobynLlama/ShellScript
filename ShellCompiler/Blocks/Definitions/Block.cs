using ShellCompiler.Statements;

namespace ShellCompiler.Blocks;

public abstract partial class Block : IBlock
{
  public uint Depth { get; set; } = 0u;
  public abstract Statement AssembleBlock(Queue<IToken> tokens);
  public override string ToString()
  {
    return GetType().Name;
  }
}
