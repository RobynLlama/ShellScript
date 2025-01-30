using ShellCompiler.Blocks;
using ShellCompiler.Statements;

namespace ShellCompiler;

public static partial class ShellAssembler
{
  public static Statement[] CompileAssembly(Queue<IToken> input)
  {

    List<Statement> statements = [];
    Queue<IToken> secondPass = [];

    Console.WriteLine($"Resolving variable literals [{input.Count}]");

    while (input.Count > 0)
    {
      var current = input.Dequeue();

      if (current is IKeyword kw && kw.Symbol == ReservedSymbol.VARIABLE)
      {
        secondPass.Enqueue(Synthesizers.AssembleReadVariable(input));
        continue;
      }

      secondPass.Enqueue(current);
    }

    Console.WriteLine($"Performing second pass [{secondPass.Count}]");

    while (secondPass.Count > 0)
    {
      var current = secondPass.Dequeue();
      Console.WriteLine($"Processing {current}");

      if (current is not IBlock next)
      {
        if (current is IKeyword kw)
        {
          if (kw.Symbol != ReservedSymbol.STATEMENT_TERMINATOR)
            throw new InvalidOperationException($"Non build token encountered in stream {kw}");
          else
            continue;
        }
        throw new InvalidOperationException($"Non build token encountered in stream {current}");
      }

      statements.Add(next.AssembleBlock(secondPass));
    }

    return [.. statements];
  }
}
