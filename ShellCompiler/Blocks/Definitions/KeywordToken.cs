namespace ShellCompiler.Blocks;

public abstract class KeywordToken : IKeyword
{
  public bool DepthIncrease { get; protected set; } = false;
  public bool DepthDecrease { get; protected set; } = false;
  public ReservedSymbol Symbol { get; protected set; } = ReservedSymbol.STATEMENT_TERMINATOR;
  public uint Depth { get; set; } = 0u;
  public override string ToString()
  {
    return Symbol.ToString();
  }
}
