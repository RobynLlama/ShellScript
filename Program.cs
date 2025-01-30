// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Text;
using ShellCompiler;
using ShellCompiler.Statements;

//Console.WriteLine("Shellscript Interpreter");
Stopwatch clock = new();

Dictionary<string, string> Variables = [];

string script =
"""
#My first script!
echo "My first Script"
ls #list the directory
cd /user/me/coolStuff
Var = "thing"
cat >> output.txt
cat < input.txt
ls|cat

IF ($Var == "thing")
  echo "$Var"
  Var = "cool!"
END

echo "Script done!"

""";

clock.Start();

var tokens = ShellAssembler.TokenizeString(script);
Console.WriteLine($"Script tokenized in {clock.ElapsedMilliseconds} ms");

clock.Restart();

var blocks = ShellAssembler.ParseTokens(tokens);

Console.WriteLine($"Tokens parsed in {clock.ElapsedMilliseconds} ms");

clock.Restart();

var assembly = ShellAssembler.CompileAssembly(blocks);

Console.WriteLine($"Assembly synthesized in {clock.ElapsedMilliseconds} ms");

foreach (var item in assembly)
{
  Console.WriteLine(item.GetType().Name);

  if (item is IFStatement fi)
  {
    foreach (var thing in fi.Run)
    {
      Console.WriteLine($"  {thing.GetType().Name}");
    }
  }
}
