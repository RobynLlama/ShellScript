using ShellCompiler.Statements;
using ShellCompiler.Blocks;
using org.matheval;

namespace ShellCompiler;

public static partial class Synthesizers
{
  public static Expression AssembleConditionExpression(Queue<IToken> tokens)
  {
    //Condition needs
    //  open_parenthesis
    //  close_parenthesis

    if (tokens.Count == 0)
      throw new InvalidOperationException("Unexpected end of file in AssembleConditionExpression");

    IToken next;

    next = tokens.Dequeue();

    if (next is not Operator op || op.Symbol != ReservedSymbol.PARENTHESIS_OPEN)
      throw new InvalidOperationException($"Open parenthesis expected in AssembleConditionExpression. Block: {next}");

    Queue<IToken> expressionTokens = new();

    while (tokens.Count > 0)
    {
      next = tokens.Dequeue();

      if (next is Operator op2 && op2.Symbol == ReservedSymbol.PARENTHESIS_CLOSE)
        break;

      expressionTokens.Enqueue(next);
    }

    return AssembleExpressionString(expressionTokens);
  }
}
