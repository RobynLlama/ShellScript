using System.Text.RegularExpressions;
using ShellCompiler.Statements;

namespace ShellCompiler.Blocks;

/// <summary>
/// A block containing a literal string value
/// </summary>
/// <param name="input"></param>
public partial class Literal(string input) : RunnableBlock
{
  public readonly string Value = input;
  public override string RawValue { get => Value; }

  /// <summary>
  /// Parses the value of the literal and expands variables if it is not
  /// inside a single quoted string
  /// </summary>
  /// <param name="shell"></param>
  /// <returns></returns>
  public override string GetParsedValue(ShellExecutable shell)
  {
    string output = Value;

    Utils.BindVariableNames(shell, ref output);

    return output;
  }
}
