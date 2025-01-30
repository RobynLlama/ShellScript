namespace ShellCompiler.Statements;

public abstract class Statement
{
  public virtual object? Execute(ShellExecutable shell) => null;
}
