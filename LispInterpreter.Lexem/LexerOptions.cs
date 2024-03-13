namespace LispInterpreter.Lexems;

class LexerOptions
{
    public readonly char LeftBracketCharacter;
    
    public readonly char RightBracketCharacter;

    public LexerOptions(char leftBracketCharacter, char rightBracketCharacter)
    {
        LeftBracketCharacter = leftBracketCharacter;
        RightBracketCharacter = rightBracketCharacter;
    }
}