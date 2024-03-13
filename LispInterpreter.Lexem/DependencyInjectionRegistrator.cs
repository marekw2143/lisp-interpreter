using Microsoft.Extensions.DependencyInjection;

namespace LispInterpreter.Lexems;

public static class DependencyInjectionRegistrator
{
    public static ServiceCollection RegisterLexicalParser(this ServiceCollection services)
    {
        services.AddSingleton<ILexicalParser, LexicalParser>();
        services.AddSingleton(new LexerOptions('(', ')'));
        return services;
    }
}