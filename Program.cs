// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using ShellCompiler;

//Console.WriteLine("Shellscript Interpreter");
Stopwatch clock = new();

Dictionary<string, string> Variables = [];

string script =
"""
#My first script!
echo "My first Script"
ls #list the directory
cd "/user/me/coolStuff"
Var = 12.5
cat >> output.txt
cat < input.txt
ls|cat

IF ($Var + 1 == 13.5)
  echo "$Var"
  Var = "cool!"
END

echo "Script done!"

""";

ShellExecutable waah = new(runNoReturn, runWithReturn, GetVariableMethod, SetVariableMethod);

void SetVariableMethod(string variableName, string value)
{
  Console.WriteLine($"SetVariable: {variableName}, {value}");
  Variables[variableName] = value;
}

string GetVariableMethod(string variableName)
{
  Console.WriteLine($"GetVariable: {variableName}");

  if (Variables.TryGetValue(variableName, out var thing))
    return thing;

  return string.Empty;
}

object? runWithReturn(string application, string[] args)
{
  Console.WriteLine($"RunWithReturn: {application}");
  foreach (var item in args)
  {
    Console.WriteLine($"  arg: {item}");
  }

  return null;
}

void runNoReturn(string application, string[] args)
{
  Console.WriteLine($"RunWithoutReturn: {application}");
  foreach (var item in args)
  {
    Console.WriteLine($"  arg: {item}");
  }
}

clock.Restart();
waah.CompileProgram(script);
Console.WriteLine($"Program compiled and run-ready in {clock.ElapsedMilliseconds} ms");

var program = waah.RunProgram();

clock.Restart();
while (program.MoveNext())
{
}


Console.WriteLine($"Program executed in {clock.ElapsedMilliseconds} ms");
