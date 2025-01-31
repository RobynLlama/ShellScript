using System.Text.RegularExpressions;

namespace ShellCompiler;

public static partial class Utils
{
  /// <summary>
  /// Pregenerated Regex to match variables
  /// </summary>
  /// <returns></returns>
  private static readonly Regex variableMatcher = MyRegex();

  [GeneratedRegex(@"\$(\w+)")]
  private static partial Regex MyRegex();
  public static string[] GetVariableNames(string input)
  {
    var matches = variableMatcher.Matches(input);
    List<string> variables = [];

    if (matches.Count == 0)
      return [];

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

    return [.. variables];
  }

  public static void BindVariableNames(ShellExecutable shell, ref string input, string[] variables)
  {
    foreach (var item in variables)
      input = input.Replace($"${item}", shell.GetVariable(item).GetValueForTerminal());
  }

  public static void BindVariableNames(ShellExecutable shell, ref string input, bool preserveQuotes = false) => BindVariableNames(shell, ref input, GetVariableNames(input));
}
