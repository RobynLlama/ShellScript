using System;
using System.Diagnostics.CodeAnalysis;
using ShellCompiler.Statements;

namespace ShellCompiler.Blocks;

public abstract class Keyword : Block
{

  public virtual bool DepthIncrease { get; } = false;
  public virtual bool DepthDecrease { get; } = false;
  public abstract ReservedSymbol Symbol { get; }

  private static readonly Dictionary<ReservedSymbol, Type> TypeTable = new()
  {
    {ReservedSymbol.IF, typeof(IFKeyword)},
    {ReservedSymbol.END, typeof(NonBuildingKeywordLowDepth)},
  };

  private static readonly Dictionary<string, ReservedSymbol> ConversionTable = new()
  {
    {"if", ReservedSymbol.IF},
    {"end", ReservedSymbol.END},
    {"else", ReservedSymbol.ELSE},
    {"switch", ReservedSymbol.SWITCH},
    {"break", ReservedSymbol.BREAK},
    {"exit", ReservedSymbol.EXIT},
    {"return", ReservedSymbol.RETURN},
    {"function", ReservedSymbol.FUNCTION},
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


  public static bool TryGetKeyword(string token, [NotNullWhen(true)] out Keyword? result)
  {
    result = null;

    if (!ConversionTable.TryGetValue(token.ToLowerInvariant(), out var symbol))
      return false;

    if (!TypeTable.TryGetValue(symbol, out var type))
    {
      //use the most generic non building type
      result = new NonBuildingKeyword(symbol);
      return true;
    }

    if (type.IsAssignableTo(typeof(NonBuildingKeyword)))
    {
      //its a generic type so we have to send it the symbol type
      if (Activator.CreateInstance(type, [symbol]) is not NonBuildingKeyword nkw)
        return false;

      result = nkw;
      return true;
    }

    //No arguments
    if (Activator.CreateInstance(type) is not Keyword kw)
      return false;

    result = kw;
    return true;
  }

  public override Statement AssembleBlock(Queue<Block> blocks) =>
    throw new NotImplementedException($"Unable to build block: {Symbol}");

}
