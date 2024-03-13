using LispInterpreter.Lexems;
using Microsoft.Extensions.DependencyInjection;

namespace LispInterpreter.AST.Tests;

public class UnitTest1
{
    private IAbstractSyntaxTreeParser _sut;

    public UnitTest1()
    {
        _sut = new ServiceCollection()
                    .RegisterASTParser()
                    .BuildServiceProvider()
                    .GetService<IAbstractSyntaxTreeParser>();
    }
    
    [Fact]
    public void SetsProperValueToVariable()
    {
        var res = _sut.TryExtractVariableDefinition(new Lexem[]
        {
            new OpeningBracketLexem(),
            new StringLexem("define"),
            new StringLexem("idx"),
            new NumberLexem(5),
            new ClosingBracketLexem()
        }, out var name, out var value);

        Assert.True(res);
        Assert.Equal("idx", name);
        Assert.Equal(5, value);
    }

    [Fact]
    public void Throws_WhenIncorrectNumberOfParameterPassed()
    {
        var expressoion =
            _sut.ParseExpression(new Lexem[]
            {
                new OpeningBracketLexem(),
                new StringLexem("/"),
                new NumberLexem(2),
                new NumberLexem(5),
                new NumberLexem(6),
                new ClosingBracketLexem()
            }, 0, out var _, new Dictionary<string, FunctionEntry>());

        Assert.Throws<SyntaxException>(() => expressoion.Evaluate(new Dictionary<string, int>()));
    }
}