using ShellCompiler.Statements;

namespace ShellCompiler.Blocks;

public interface IBlock : IToken
{
  public Statement AssembleBlock(Queue<IToken> tokens);
}
