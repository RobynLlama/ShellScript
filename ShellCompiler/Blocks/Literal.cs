using System.Text.RegularExpressions;
using ShellCompiler.Statements;

namespace ShellCompiler.Blocks;

/// <summary>
/// A block containing a literal value, possibly single or double quoted
/// </summary>
/// <param name="input"></param>
public partial class Literal(string input) : Block
{
  public readonly string Value = input;
  public bool IsVariableLiteral = false;

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
  public string GetParsedValue(ShellExecutable shell)
  {

    if (IsVariableLiteral)
      return shell.GetVariable(Value);

    //Parse the value if it isn't inside a single quoted string
    if (!Value.StartsWith('\''))
    {
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
    }

    return Value;
  }

  public override Statement AssembleBlock(Queue<IToken> tokens)
  {

    if (tokens.Count == 0)
      throw new InvalidOperationException("Unexpected end of file while creating literal");

    var next = tokens.Peek();

    if (next is IKeyword kw && kw.Symbol == ReservedSymbol.ASSIGNMENT)
    {
      tokens.Dequeue();

      if (tokens.Count == 0)
        throw new InvalidOperationException("Unexpected end of file in Literal.AssembleBlock");

      next = tokens.Dequeue();

      if (next is not Literal lit)
        throw new InvalidOperationException($"Expected literal in Literal.AssembleBlock. Block: {next.GetType().Name}");

      return new VariableAssignment(this, lit);
    }


    List<Literal> arguments = [];

    while (tokens.Count > 0)
    {
      next = tokens.Dequeue();

      if (next is not Literal lit)
      {
        if (next is not IKeyword key)
          throw new InvalidOperationException($"Something other than a literal in arguments list of binary literal {next.GetType().Name}");

        switch (key.Symbol)
        {
          case ReservedSymbol.GREATER_THAN:
          case ReservedSymbol.DOUBLE_GREATER_THAN:
          case ReservedSymbol.LESS_THAN:
          case ReservedSymbol.DOUBLE_LESS_THAN:
          case ReservedSymbol.PIPE:
            //Console.WriteLine("Consumed a redirect symbol");
            continue;
          case ReservedSymbol.STATEMENT_TERMINATOR:
            goto finish;
          default:
            throw new InvalidOperationException($"Unexpected keyword in literal binary arguments list {key.Symbol}");
        }
      }

      arguments.Add(lit);
    }

  finish:

    return new RunBinary(this, [.. arguments]);

  }
}
