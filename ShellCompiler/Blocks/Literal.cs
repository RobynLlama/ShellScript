using System.Text.RegularExpressions;
using ShellCompiler.Statements;

namespace ShellCompiler.Blocks;

/// <summary>
/// A block containing a literal string value
/// </summary>
/// <param name="input"></param>
public partial class Literal : RunnableBlock
{
  public readonly string Value;
  public readonly bool NeedsExpansion = false;
  public readonly bool IsStringLiteral = false;
  public override string ExpressionValue
  {
    get
    {
      if (IsStringLiteral)
        return '"' + Value + '"';

      return Value;
    }
  }

  public Literal(string input)
  {
    if (input.StartsWith('"'))
    {
      NeedsExpansion = true;
      IsStringLiteral = true;
    }

    if (input.StartsWith('\''))
      IsStringLiteral = true;

    Value = input.Trim('"').Trim('\'');
  }

  /// <summary>
  /// Parses the value of the literal and expands variables if it is not
  /// inside a single quoted string
  /// </summary>
  /// <param name="shell"></param>
  /// <returns></returns>
  public override string GetParsedValue(ShellExecutable shell)
  {
    string output = Value;

    if (NeedsExpansion)
      Utils.BindVariableNames(shell, ref output);

    return output.Trim('"').Trim('\'');
  }
}
