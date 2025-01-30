namespace ShellCompiler.Blocks;

public class NonBuildingKeyword(ReservedSymbol symbol) : Keyword
{
  public override ReservedSymbol Symbol => symbol;
}
