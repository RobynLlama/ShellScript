using System.Diagnostics.CodeAnalysis;

namespace ShellCompiler.Blocks;

public static class Symbol
{
  private static readonly Dictionary<ReservedSymbol, Type> TypeTable = new()
  {
    {ReservedSymbol.IF, typeof(IFKeyword)},
    {ReservedSymbol.END, typeof(GenericKeywordLowDepth)},
  };

  private static readonly Dictionary<string, ReservedSymbol> KeywordConversionTable = new()
  {
    {"if", ReservedSymbol.IF},
    {"end", ReservedSymbol.END},
    {"else", ReservedSymbol.ELSE},
    {"switch", ReservedSymbol.SWITCH},
    {"break", ReservedSymbol.BREAK},
    {"exit", ReservedSymbol.EXIT},
    {"return", ReservedSymbol.RETURN},
    {"function", ReservedSymbol.FUNCTION},
  };

  private static readonly Dictionary<string, ReservedSymbol> OperatorConversionTable = new()
  {
    {">", ReservedSymbol.GREATER_THAN},
    {">=", ReservedSymbol.GREATER_OR_EQUAL},
    {">>", ReservedSymbol.DOUBLE_GREATER_THAN},
    {"<", ReservedSymbol.LESS_THAN},
    {"<=", ReservedSymbol.LESS_OR_EQUAL},
    {"<<", ReservedSymbol.DOUBLE_LESS_THAN},
    {"|", ReservedSymbol.PIPE},
    {"||", ReservedSymbol.DOUBLE_PIPE},
    {"=", ReservedSymbol.ASSIGNMENT},
    {"==", ReservedSymbol.COMPARISON},
    {"!=", ReservedSymbol.NOT_COMPARISON},
    {"$", ReservedSymbol.VARIABLE},
    {"{", ReservedSymbol.CURLY_OPEN},
    {"}", ReservedSymbol.CURLY_CLOSE},
    {"(", ReservedSymbol.PARENTHESIS_OPEN},
    {")", ReservedSymbol.PARENTHESIS_CLOSE},
    {"[", ReservedSymbol.BRACKET_OPEN},
    {"]", ReservedSymbol.BRACKET_CLOSE},
    {";", ReservedSymbol.STATEMENT_TERMINATOR},
  };

  public static bool TryGetKeyword(string token, [NotNullWhen(true)] out ISymbol? result)
  {
    result = null;

    //Console.WriteLine($"Retrieving symbol for {token}");

    if (!KeywordConversionTable.TryGetValue(token.ToLowerInvariant(), out var symbol))
    {
      if (!OperatorConversionTable.TryGetValue(token.ToLowerInvariant(), out var op))
        return false;

      result = new Operator(op);
      //Console.WriteLine($"Returning {op}");
      return true;
    }

    if (!TypeTable.TryGetValue(symbol, out var type))
    {
      //use the most generic non building type
      result = new GenericKeyword(symbol);
      return true;
    }

    if (type.IsAssignableTo(typeof(GenericKeyword)))
    {
      //its a generic type so we have to send it the symbol type
      if (Activator.CreateInstance(type, [symbol]) is not GenericKeyword nkw)
        return false;

      result = nkw;
      return true;
    }

    //No arguments
    if (Activator.CreateInstance(type) is not Keyword kw)
      return false;

    result = kw;
    //Console.WriteLine($"Returning: {symbol}");
    return true;
  }
}
