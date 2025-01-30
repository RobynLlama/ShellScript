using ShellCompiler.Statements;

namespace ShellCompiler.Blocks;

public abstract partial class Block
{
  public uint Depth = 0u;
  public virtual Statement AssembleBlock(Queue<Block> blocks) =>
    throw new NotImplementedException($"Unable to build block: {GetType().Name}");
}
