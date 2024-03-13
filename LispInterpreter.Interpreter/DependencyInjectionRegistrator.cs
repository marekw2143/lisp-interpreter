using LispInterpreter.AST;
using LispInterpreter.Lexems;
using Microsoft.Extensions.DependencyInjection;

namespace LispInterpreter.Interpreter;

public static class DependencyInjectionRegistrator
{
    public static ServiceCollection RegisterInterpreter(this ServiceCollection services)
    {
        services.RegisterASTParser();
        services.RegisterLexicalParser();
        services.AddSingleton<Interpreter>();

        return services;
    }
}