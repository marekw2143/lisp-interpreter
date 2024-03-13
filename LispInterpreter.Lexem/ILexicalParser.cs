using System.Collections.Immutable;

namespace LispInterpreter.Lexems;

public interface ILexicalParser
{
    ImmutableList<Lexem> Parse(string input);
}