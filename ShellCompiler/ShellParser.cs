using ShellCompiler.Blocks;

namespace ShellCompiler;

public static partial class ShellAssembler
{
  public static Queue<IToken> ParseTokens(Queue<string> unsortedTokens)
  {
    uint currentBlockDepth = 0;
    Queue<IToken> sortedTokens = [];

    while (unsortedTokens.Count > 0)
    {
      var currentToken = unsortedTokens.Dequeue();

      if (Symbol.TryGetKeyword(currentToken, out var result))
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

        sortedTokens.Enqueue(result);
        //Console.WriteLine($"Token: {currentToken} => {result.Symbol} @ {result.Depth}\n");
      }
      else
      {
        IToken block;

        if (currentToken.StartsWith('$'))
          block = new VariableBlock(currentToken)
          {
            Depth = currentBlockDepth
          };
        else
          block = new Literal(currentToken)
          {
            Depth = currentBlockDepth
          };


        sortedTokens.Enqueue(block);
        //Console.WriteLine($"Token: {currentToken} => Literal @ {block.Depth}\n");
      }
    }

    return sortedTokens;
  }
}
