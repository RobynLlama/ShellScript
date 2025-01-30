namespace ShellCompiler.Blocks;

public class NonBuildingKeywordLowDepth(ReservedSymbol symbol) : NonBuildingKeyword(symbol)
{
  public override bool DepthDecrease => true;
}
