using ShellCompiler.Statements;

namespace ShellCompiler.Blocks;

public abstract class RunnableBlock : Block
{
  public override Statement AssembleBlock(Queue<IToken> tokens)
  {
    IToken next;
    //Console.WriteLine($"Assembling: {this}");

    next = tokens.Peek();

    if (next is IKeyword kw && kw.Symbol == ReservedSymbol.ASSIGNMENT)
    {
      tokens.Dequeue();

      if (tokens.Count == 0)
        throw new InvalidOperationException("Unexpected end of file in Literal.AssembleBlock");

      next = tokens.Dequeue();

      if (next is not RunnableBlock lit)
        throw new InvalidOperationException($"Expected runnable in runnable.AssembleBlock. Block: {next.GetType().Name}");

      //Console.WriteLine("Assembly Complete as VariableAssignment");
      return new VariableAssignment(this, lit);
    }

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

    //Console.WriteLine("Assembly Complete as RunBinary");
    return new RunBinary(this, [.. arguments]);
  }

  public abstract string GetParsedValue(ShellExecutable shell);
}
