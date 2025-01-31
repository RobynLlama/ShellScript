// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using ShellCompiler;

//Console.WriteLine("Shellscript Interpreter");
Stopwatch clock = new();

Dictionary<string, VariableStore> Variables = [];

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
  echo "$Var + 1 == 13.5"
  Var = "cool!"
END

echo "Script done!"

""";

ShellExecutable waah = new(runNoReturn, runWithReturn, GetVariableMethod, SetVariableMethod);

void SetVariableMethod(string variableName, object value)
{
  //Console.WriteLine($"SetVariable: {variableName}, {value} : {value.GetType().Name}");
  Variables[variableName] = new(value);
}

VariableStore GetVariableMethod(string variableName)
{
  //Console.WriteLine($"GetVariable: {variableName}");

  if (Variables.TryGetValue(variableName, out var thing))
    return thing;

  return new(string.Empty);
}

object? runWithReturn(string application, string[] args)
{
  return null;
}

void runNoReturn(string application, string[] args)
{
  if (application == "echo")
  {
    Console.WriteLine(string.Join(' ', args));
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
