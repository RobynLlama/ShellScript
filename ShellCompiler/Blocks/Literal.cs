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

  /// <summary>
  /// Pregenerated Regex to match variables
  /// </summary>
  /// <returns></returns>
  private static readonly Regex variableMatcher = MyRegex();

  [GeneratedRegex(@"\$(\w+)")]
  private static partial Regex MyRegex();

  /// <summary>
  /// Parses the value of the literal and expands variables if it is not
  /// inside a single quoted string
  /// </summary>
  /// <param name="shell"></param>
  /// <returns></returns>
  public override string GetParsedValue(ShellExecutable shell)
  {
    //Parse the value if it isn't inside a single quoted string
    var matches = variableMatcher.Matches(Value);
    List<string> variables = [];

    if (matches.Count == 0)
      return Value;

    foreach (Match match in matches)
    {
      //check the first capture group
      if (match.Groups.Count > 1)
      {
        var inc = match.Groups[1].Value;
        if (!variables.Contains(inc))
          variables.Add(inc);
      }
    }

    string output = Value;
    foreach (var item in variables)
    {
      output = output.Replace($"${item}", shell.GetVariable(item));
    }

    return output;
  }
}
