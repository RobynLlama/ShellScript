using ShellCompiler.Statements;

namespace ShellCompiler.Blocks;

public class IFKeyword : Keyword
{
  public override bool DepthIncrease => true;
  public override ReservedSymbol Symbol => ReservedSymbol.IF;

  public override Statement AssembleBlock(Queue<Block> blocks)
  {
    //IF statement requires:
    // - Condition
    // - Statement(s) to run
    // - END block

    //Console.WriteLine("Creating conditional");

    IConditional conditional = Synthesizers.AssembleConditional(blocks);
    List<Statement> statements = [];

    //Console.WriteLine($"Accumulating statements for depth {Depth}");

    while (blocks.Count > 0)
    {
      var next = blocks.Peek();

      if (next.Depth < Depth)
        throw new InvalidOperationException($"Depth fell below IF statement depth without an END statement {next.GetType().Name} at {next.Depth}");

      blocks.Dequeue();

      if (next is Keyword kw)
      {
        if (kw.Symbol == ReservedSymbol.END)
          break;

        if (kw.Symbol == ReservedSymbol.STATEMENT_TERMINATOR)
          continue;

        statements.Add(kw.AssembleBlock(blocks));
        continue;
      }

      statements.Add(next.AssembleBlock(blocks));
      continue;

    }

    return new IFStatement(Depth, conditional, [.. statements]);
  }
}
