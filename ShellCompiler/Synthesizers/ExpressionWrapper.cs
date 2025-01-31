using org.matheval;

namespace ShellCompiler;

public class ExpressionWrapper(string expression)
{
  private readonly string Original = expression;
  private Expression BindVariables(ShellExecutable shell)
  {
    string newExpression = Original;
    var names = Utils.GetVariableNames(newExpression);

    foreach (var name in names)
    {
      newExpression = newExpression.Replace("$" + name, "_" + name);
    }

    var exp = new Expression(newExpression);

    foreach (var name in names)
    {
      var thing = shell.GetVariable(name);
      Console.WriteLine($"Binding {name} to {thing.GetValueForExpression()}");
      exp.Bind("_" + name, thing.GetValueForExpression());
    }

    return exp;
  }

  public bool RunAsConditional(ShellExecutable shell) => BindVariables(shell).Eval<bool>();
  public object Eval(ShellExecutable shell) => BindVariables(shell).Eval();
}
