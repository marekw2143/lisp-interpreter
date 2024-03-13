using Microsoft.Extensions.DependencyInjection;

namespace LispInterpreter.AST;

public static class DependencyInjectionRegistrator
{
    public static ServiceCollection RegisterASTParser(this ServiceCollection services)
    {
        services.AddSingleton<IBuiltinExpressionsFactory, BuiltinExpressionsFactory>();
        services.AddSingleton<IAbstractSyntaxTreeParser, AbstractSyntaxTreeParser>();
        
        return services;
    }
}