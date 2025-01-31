namespace ShellCompiler.Blocks;

public class Operator : SymbolToken
{
  public Operator(ReservedSymbol symbol)
  {
    Symbol = symbol;
  }
}
