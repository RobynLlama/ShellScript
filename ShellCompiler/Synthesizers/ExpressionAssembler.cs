using System.Text;
using org.matheval;
using ShellCompiler.Blocks;

namespace ShellCompiler;

public static partial class Synthesizers
{
  public static ExpressionWrapper AssembleExpressionString(Queue<IToken> tokens)
  {
    //Expression assembles:
    //  -No  keywords
    //  +Yes Literals
    //  +Yes Variables
    //  +Yes Operators
    //  Stops at: Terminator

    if (tokens.Count == 0)
      throw new InvalidOperationException("Unexpected end of file in AssembleExpressionString");

    StringBuilder fullExpression = new();
    char delimiter = ' ';
    bool anything = false;
    IToken next;

    void AddToExpression(string item)
    {
      fullExpression.Append(delimiter);
      fullExpression.Append(item);
      anything = true;
    }

    while (tokens.Count > 0)
    {
      next = tokens.Peek();

      if (next is Keyword kw)
        throw new InvalidOperationException($"Unexpected Keyword in AssembleExpressionString {kw}");

      tokens.Dequeue();

      if (next is RunnableBlock rb)
      {
        AddToExpression(rb.ExpressionValue);
      }
      else if (next is Operator op)
      {
        if (op.Symbol == ReservedSymbol.STATEMENT_TERMINATOR)
          break;

        AddToExpression(op.ExpressionText);
      }
    }

    if (!anything)
      throw new InvalidOperationException("Empty expression in AssembleExpressionString");

    //Console.WriteLine(fullExpression);

    return new(fullExpression.ToString());
  }
}
