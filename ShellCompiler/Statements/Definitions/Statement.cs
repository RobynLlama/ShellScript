namespace ShellCompiler.Statements;

public abstract class Statement
{
  public abstract void Execute(ShellExecutable shell);
}
