namespace ShellCompiler.Blocks;

public class GenericKeywordLowDepth : GenericKeyword
{
  public GenericKeywordLowDepth(ReservedSymbol symbol) : base(symbol)
  {
    DepthDecrease = true;
  }
}
