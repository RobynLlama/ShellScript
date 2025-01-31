using ShellCompiler.Blocks;
using ShellCompiler.Statements;

namespace ShellCompiler;

public static partial class ShellAssembler
{
  public static Statement[] CompileAssembly(Queue<IToken> input)
  {

    List<Statement> statements = [];

    Console.WriteLine($"Assembling [{input.Count}] tokens");

    while (input.Count > 0)
    {
      var current = input.Dequeue();
      //Console.WriteLine($"Processing {current}");

      if (current is not IBlock next)
      {
        if (current is ISymbol kw)
        {
          if (kw.Symbol != ReservedSymbol.STATEMENT_TERMINATOR)
            throw new InvalidOperationException($"Non build token encountered in stream {kw}");
          else
            continue;
        }
        throw new InvalidOperationException($"Non build token encountered in stream {current}");
      }

      statements.Add(next.AssembleBlock(input));
    }

    return [.. statements];
  }
}
