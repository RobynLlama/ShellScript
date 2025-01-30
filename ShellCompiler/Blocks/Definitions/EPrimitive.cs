namespace ShellCompiler.Blocks;

public enum Primitive
{
  STRING,
  DOUBLE,
  LONG,
  BOOL
}

public static class PrimitiveExt
{
  public static Type GetBackingType(this Primitive self)
  {
    return self switch
    {
      Primitive.STRING => typeof(string),
      Primitive.DOUBLE => typeof(double),
      Primitive.LONG => typeof(long),
      Primitive.BOOL => typeof(bool),
      _ => typeof(int),
    };
  }
}
