using ShellCompiler.Blocks;
using ShellCompiler.Statements;

namespace ShellCompiler;

public static partial class ShellAssembler
{
  public static Statement[] CompileAssembly(Queue<Block> blocks)
  {

    List<Statement> statements = [];
    Queue<Block> secondPass = [];

    Console.WriteLine("Resolving variable literals");

    while (blocks.Count > 0)
    {
      var current = blocks.Dequeue();

      if (current is Keyword kw && kw.Symbol == ReservedSymbol.VARIABLE)
      {
        secondPass.Enqueue(Synthesizers.AssembleReadVariable(blocks));
        continue;
      }

      secondPass.Enqueue(current);
    }

    Console.WriteLine("Performing second pass");

    while (secondPass.Count > 0)
    {
      var current = secondPass.Dequeue();

      if (current is NonBuildingKeyword kw)
      {
        if (kw.Symbol == ReservedSymbol.STATEMENT_TERMINATOR)
          continue;
      }

      statements.Add(current.AssembleBlock(secondPass));
    }

    return [.. statements];
  }
}
