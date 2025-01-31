using ShellCompiler.Statements;

namespace ShellCompiler.Blocks;

public class VariableBlock(string input) : RunnableBlock
{
  public string Varname = input[1..];
  public override string GetParsedValue(ShellExecutable shell) =>
    shell.GetVariable(Varname);
  public override Statement AssembleBlock(Queue<IToken> tokens)
  {
    Console.WriteLine("Assembling: VariableReader");
    //If there are no tokens this isn't an assignment
    if (tokens.Count == 0)
      return base.AssembleBlock(tokens);

    var next = tokens.Peek();

    if (next is IKeyword kw && kw.Symbol == ReservedSymbol.ASSIGNMENT)
    {
      tokens.Dequeue();

      if (tokens.Count == 0)
        throw new InvalidOperationException("Unexpected end of file in Literal.AssembleBlock");

      next = tokens.Dequeue();

      if (next is not RunnableBlock lit)
        throw new InvalidOperationException($"Expected literal in Literal.AssembleBlock. Block: {next.GetType().Name}");

      Console.WriteLine("Assembly Complete");
      return new VariableAssignment(this, lit);
    }

    return base.AssembleBlock(tokens);
  }
}
