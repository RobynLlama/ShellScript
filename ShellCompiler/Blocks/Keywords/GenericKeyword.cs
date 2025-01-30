namespace ShellCompiler.Blocks;

public class GenericKeyword : KeywordToken
{
  public GenericKeyword(ReservedSymbol symbol)
  {
    Symbol = symbol;
  }
}
