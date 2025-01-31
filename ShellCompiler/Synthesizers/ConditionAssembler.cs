using ShellCompiler.Statements;
using ShellCompiler.Blocks;

namespace ShellCompiler;

public static partial class Synthesizers
{
  public static IConditional AssembleConditional(Queue<IToken> tokens)
  {
    //Condition needs
    //  open_parenthesis
    //  Left hand literal
    //  one of: ==, >=, <=, >, <
    //  Right hand literal
    //  close_parenthesis

    RunnableBlock? left = null;
    ReservedSymbol symbol = ReservedSymbol.STATEMENT_TERMINATOR;
    RunnableBlock? right = null;
    bool par_closed = false;
    bool par_opened = false;

    while (tokens.Count > 0)
    {

      var current = tokens.Dequeue();
      //Console.WriteLine(current.GetType().Name);

      /*
      if (current is KeywordBlock ckw)
      {
        Console.WriteLine(ckw.Symbol);
      }
      */

      if (!par_opened)
      {
        if (current is not ISymbol kw || kw.Symbol != ReservedSymbol.PARENTHESIS_OPEN)
          throw new InvalidOperationException($"Expecting an open parenthesis while building conditional. Block: {current.GetType().Name}");

        par_opened = true;
        continue;
      }

      if (left is null)
      {
        if (current is not RunnableBlock lw)
          throw new InvalidOperationException($"Expected literal as left hand term while building conditional. Block: {current.GetType().Name}");

        left = lw;
        continue;
      }

      if (symbol == ReservedSymbol.STATEMENT_TERMINATOR)
      {
        if (current is not ISymbol kw)
          throw new InvalidOperationException($"Expected keyword while building conditional. Block: {current.GetType().Name}");

        symbol = kw.Symbol;
        continue;
      }

      if (right is null)
      {
        if (current is not RunnableBlock lw)
          throw new InvalidOperationException($"Expected literal as right hand term while building conditional. Block: {current.GetType().Name}");

        right = lw;
        continue;
      }

      if (!par_closed)
      {
        if (current is not ISymbol kw || kw.Symbol != ReservedSymbol.PARENTHESIS_CLOSE)
          throw new InvalidOperationException($"Expecting an close parenthesis while building conditional. Block: {current.GetType().Name}");

        par_closed = true;
        break;
      }

    }

    if (left is null || symbol == ReservedSymbol.STATEMENT_TERMINATOR || right is null || !par_closed)
    {
      throw new InvalidOperationException("Unexpected end of file while building conditional");
    }

    switch (symbol)
    {
      case ReservedSymbol.COMPARISON:
        return new EqualityStatement(left, right);
      default:
        throw new InvalidOperationException($"Invalid symbol in conditional {symbol}");
    }
  }
}
