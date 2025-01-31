namespace ShellCompiler.Blocks;

public abstract class Keyword : Block, ISymbol
{
  public bool DepthIncrease { get; protected set; } = false;
  public bool DepthDecrease { get; protected set; } = false;
  public ReservedSymbol Symbol { get; protected set; }
}
