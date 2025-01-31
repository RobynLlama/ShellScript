namespace ShellCompiler;

public class VariableStore(object value)
{
  private readonly object Value = value;
  public object GetValueForExpression()
  {
    if (Value is string item)
      return '"' + item + '"';

    return Value;
  }

  public string GetValueForTerminal() => Value.ToString() ?? string.Empty;
}
