namespace ShellCompiler.Blocks;

public interface ISymbol : IToken
{
  public bool DepthIncrease { get; }
  public bool DepthDecrease { get; }
  public ReservedSymbol Symbol { get; }
}
