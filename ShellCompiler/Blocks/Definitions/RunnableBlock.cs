using ShellCompiler.Statements;

namespace ShellCompiler.Blocks;

public abstract class RunnableBlock : Block
{
  public abstract string ExpressionValue { get; }
  public override Statement AssembleBlock(Queue<IToken> tokens)
  {
    IToken next;
    //Console.WriteLine($"Assembling: {this}");

    if (tokens.TryPeek(out var nextOp) && nextOp is Operator kw && kw.Symbol == ReservedSymbol.ASSIGNMENT)
    {
      tokens.Dequeue();

      return new VariableAssignment(this, Synthesizers.AssembleExpressionString(tokens));
    }

    List<RunnableBlock> arguments = [];

    while (tokens.Count > 0)
    {
      next = tokens.Dequeue();

      if (next is not RunnableBlock rToken)
      {
        if (next is not Operator key)
          throw new InvalidOperationException($"Something other than a literal in arguments list of binary literal {next.GetType().Name}");

        switch (key.Symbol)
        {
          case ReservedSymbol.GREATER_THAN:
          case ReservedSymbol.DOUBLE_GREATER_THAN:
          case ReservedSymbol.LESS_THAN:
          case ReservedSymbol.PIPE:
            //Console.WriteLine("Consumed a redirect symbol");
            continue;
          case ReservedSymbol.STATEMENT_TERMINATOR:
            goto finish;
          default:
            throw new InvalidOperationException($"Unexpected operator in runnable binary arguments list {key.Symbol}");
        }
      }

      arguments.Add(rToken);
    }

  finish:

    //Console.WriteLine("Assembly Complete as RunBinary");
    return new RunBinary(this, [.. arguments]);
  }

  public abstract string GetParsedValue(ShellExecutable shell);
  public override string ToString()
  {
    return $"{GetType().Name}:{ExpressionValue}";
  }
}
