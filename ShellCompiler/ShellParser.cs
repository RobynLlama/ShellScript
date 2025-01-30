using ShellCompiler.Blocks;

namespace ShellCompiler;

public static partial class ShellAssembler
{
  public static Queue<Block> ParseTokens(Queue<string> tokens)
  {
    uint currentBlockDepth = 0;
    Queue<Block> blocks = [];

    while (tokens.Count > 0)
    {
      var currentToken = tokens.Dequeue();

      if (Keyword.TryGetKeyword(currentToken, out var result))
      {

        if (result.DepthIncrease)
          result.Depth = ++currentBlockDepth;
        else if (result.DepthDecrease)
        {
          if (currentBlockDepth > 0)
            result.Depth = currentBlockDepth--;
          else
            throw new InvalidOperationException($"Block attempted to decrease depth below zero {result.GetType().Name}");
        }
        else
        {
          result.Depth = currentBlockDepth;
        }

        blocks.Enqueue(result);
        //Console.WriteLine($"Token: {currentToken} => {result.Symbol} @ {result.Depth}\n");
      }
      else
      {
        var block = new Literal(currentToken)
        {
          Depth = currentBlockDepth
        };
        blocks.Enqueue(block);
        //Console.WriteLine($"Token: {currentToken} => Literal @ {block.Depth}\n");
      }
    }

    return blocks;
  }
}
