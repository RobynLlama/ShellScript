namespace ShellCompiler.Blocks;

public interface IConditional
{
  bool Evaluate(ShellExecutable shell);
}
