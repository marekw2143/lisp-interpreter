namespace LispInterpreter.Lexems;

public sealed class StringLexem : Lexem
{
    public StringLexem(string value)
    {
        Value = value;
    }

    public readonly string Value;
}