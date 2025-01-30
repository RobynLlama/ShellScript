using System.Collections;

using ShellCompiler.Statements;

namespace ShellCompiler;

public delegate object? RunApplicationWithReturn(string application, string[] args);
public delegate void RunApplicationNoReturn(string application, string[] args);
public delegate string GetVariable(string variableName);
public delegate void SetVariable(string variableName, string value);

public class ShellExecutable(RunApplicationNoReturn runNoReturnDelegate, RunApplicationWithReturn runWithReturnDelegate, GetVariable getVarDelegate, SetVariable setVarDelegate)
{
  public readonly RunApplicationNoReturn RunWithoutReturn = runNoReturnDelegate;
  public readonly RunApplicationWithReturn RunWithReturn = runWithReturnDelegate;
  public readonly GetVariable GetVariable = getVarDelegate;
  public readonly SetVariable SetVariable = setVarDelegate;

  private Statement[] _program = [];

  public IEnumerator RunProgram()
  {
    foreach (var item in _program)
    {
      item.Execute(this);
      yield return 0;
    }
  }

  public void CompileProgram(string input)
  {
    _program = ShellAssembler.CompileAssembly(ShellAssembler.ParseTokens(ShellAssembler.TokenizeString(input)));
  }
}
