using ShellCompiler.Statements;

namespace ShellCompiler.Blocks;

public abstract class RunnableBlock : Block
{
  public override Statement AssembleBlock(Queue<IToken> tokens)
  {
    IToken next;
    Console.WriteLine("Assembling: RunnableToken");

    List<RunnableBlock> arguments = [];

    while (tokens.Count > 0)
    {
      next = tokens.Dequeue();

      if (next is not RunnableBlock rToken)
      {
        if (next is not IKeyword key)
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
            throw new InvalidOperationException($"Unexpected keyword in literal binary arguments list {key.Symbol}");
        }
      }

      arguments.Add(rToken);
    }

  finish:

    Console.WriteLine("Assembly Complete");
    return new RunBinary(this, [.. arguments]);
  }

  public abstract string GetParsedValue(ShellExecutable shell);
}
