using System.Text;

namespace ShellCompiler;

public static partial class ShellAssembler
{
  public static Queue<string> TokenizeString(string input)
  {

    Queue<string> tokens = [];
    StringBuilder buffer = new();
    Queue<char> stream = new(input);

    void SaveAndClearBuffer()
    {
      if (buffer.Length > 0)
      {
        tokens.Enqueue(buffer.ToString());
        buffer.Clear();
      }
    }

    bool PeekCompareNext(char[] matchOn)
    {
      if (!stream.TryPeek(out var next))
        return false;

      foreach (char item in matchOn)
      {
        if (item == next)
        {
          return true;
        }
      }

      return false;
    }

    while (stream.Count > 0)
    {

      var current = stream.Dequeue();

      switch (current)
      {
        //Reset current token on space
        case '\t':
        case ' ':
          //this token breaks tokens before it
          //this token is not saved because it is a delimiter
          SaveAndClearBuffer();
          break;

        //Reserved characters that can grow
        case '=':
          //this token can grow and become ==
          //this token breaks tokens before and after it
          SaveAndClearBuffer();

          buffer.Append(current);
          if (PeekCompareNext(['=']))
          {
            buffer.Append(stream.Dequeue());
          }

          SaveAndClearBuffer();
          break;
        case '|':
          //this token can grow and become ||
          //this token breaks tokens before and after it
          SaveAndClearBuffer();

          buffer.Append(current);
          if (PeekCompareNext(['|']))
          {
            buffer.Append(stream.Dequeue());
          }

          SaveAndClearBuffer();
          break;
        case '<':
          //this token can grow and become <= or <<
          //this token breaks tokens before and after it
          SaveAndClearBuffer();

          buffer.Append(current);
          if (PeekCompareNext(['<', '=']))
          {
            buffer.Append(stream.Dequeue());
          }

          SaveAndClearBuffer();
          break;
        case '>':
          //this token can grow and become >= or >>
          //this token breaks tokens before and after it
          SaveAndClearBuffer();

          buffer.Append(current);
          if (PeekCompareNext(['=', '>']))
          {
            buffer.Append(stream.Dequeue());
          }

          SaveAndClearBuffer();
          break;

        //Reserved characters that cannot grow
        case '$':
        case '{':
        case '}':
        case '[':
        case ']':
        case '(':
        case ')':
          //these tokens split before and after themselves
          SaveAndClearBuffer();
          buffer.Append(current);
          SaveAndClearBuffer();
          break;

        //Comments
        case '#':
          //this token can grow until terminated
          //this token breaks tokens before it
          //this token is discarded

          SaveAndClearBuffer();

          while (!PeekCompareNext(['\n', '\r']))
          {
            stream.Dequeue();
          }
          break;

        //handle quoted input
        case '"':
        case '\'':
          //this token can grow until terminated
          //this token breaks tokens before and after it

          SaveAndClearBuffer();
          buffer.Append(current);

          //terminates on line ending or another single/double quote
          while (!PeekCompareNext([current, '\n', '\r']))
          {
            buffer.Append(stream.Dequeue());
          }

          //ended on the correct terminator
          if (PeekCompareNext([current]))
          {
            stream.Dequeue();
            buffer.Append(current);
            break;
          }

          //Ended on \n or \r
          throw new InvalidOperationException($"Line ending inside of string literal, expected {current}");

        //line returns
        case '\n':
        case '\r':
        case ';':
          //this token breaks tokens before it
          SaveAndClearBuffer();
          tokens.Enqueue(";");
          break;

        //everything else
        default:
          buffer.Append(current);
          break;
      }
    }

    if (buffer.Length > 0)
      tokens.Enqueue(buffer.ToString());

    return tokens;
  }
}
