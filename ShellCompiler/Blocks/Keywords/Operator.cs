namespace ShellCompiler.Blocks;

public class Operator : SymbolToken
{

  public readonly string ExpressionText;

  public Operator(ReservedSymbol symbol, string expressionText)
  {
    Symbol = symbol;
    ExpressionText = expressionText;
  }
}
