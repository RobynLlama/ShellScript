namespace ShellCompiler.Blocks;

public interface IKeyword : IToken
{
  public bool DepthIncrease { get; }
  public bool DepthDecrease { get; }
  public ReservedSymbol Symbol { get; }
}
