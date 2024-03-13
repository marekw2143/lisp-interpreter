namespace LispInterpreter.Lexems;

/// <summary>
/// Represents number literal.
/// </summary>
public sealed class NumberLexem : Lexem
{
    public NumberLexem(int value)
    {
        Value = value;
    }

    public readonly int Value;
}