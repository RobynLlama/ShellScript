namespace ShellCompiler.Blocks;

public class GenericKeyword : SymbolToken
{
  public GenericKeyword(ReservedSymbol symbol)
  {
    Symbol = symbol;
  }
}
