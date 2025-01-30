using ShellCompiler.Statements;

namespace ShellCompiler.Blocks;

public class IFKeyword : KeywordBlock
{
  public IFKeyword()
  {
    DepthIncrease = true;
    Symbol = ReservedSymbol.IF;
  }

  public override Statement AssembleBlock(Queue<IToken> tokens)
  {
    //IF statement requires:
    // - Condition
    // - Statement(s) to run
    // - END block

    //Console.WriteLine("Creating conditional");

    IConditional conditional = Synthesizers.AssembleConditional(tokens);
    List<Statement> statements = [];

    //Console.WriteLine($"Accumulating statements for depth {Depth}");

    while (tokens.Count > 0)
    {
      var next = tokens.Peek();

      if (next.Depth < Depth)
        throw new InvalidOperationException($"Depth fell below IF statement depth without an END statement {next.GetType().Name} at {next.Depth}");

      tokens.Dequeue();

      if (next is IKeyword kw)
      {
        if (kw.Symbol == ReservedSymbol.END)
          break;

        if (kw.Symbol == ReservedSymbol.STATEMENT_TERMINATOR)
          continue;
      }

      if (next is IBlock block)
      {
        statements.Add(block.AssembleBlock(tokens));
        continue;
      }


      throw new InvalidOperationException($"Non build token encountered in stream {next}");
    }

    return new IFStatement(Depth, conditional, [.. statements]);
  }
}
