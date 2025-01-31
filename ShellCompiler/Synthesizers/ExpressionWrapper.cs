using org.matheval;

namespace ShellCompiler;

public class ExpressionWrapper(string expression)
{
  private readonly string Original = expression;
  private Expression BindVariables(ShellExecutable shell)
  {
    string newExpression = Original;

    Utils.BindVariableNames(shell, ref newExpression, preserveQuotes: true);

    //Console.WriteLine(newExpression);

    return new(newExpression);
  }

  public bool RunAsConditional(ShellExecutable shell) => BindVariables(shell).Eval<bool>();
  public string Eval(ShellExecutable shell) => BindVariables(shell).Eval<string>();
}
