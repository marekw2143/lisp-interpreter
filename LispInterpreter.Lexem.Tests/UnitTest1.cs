using LispInterpreter.Lexems;
using Microsoft.Extensions.DependencyInjection;

namespace LispInterpreter.Lexem.Tests;

public class UnitTest1
{

    private ILexicalParser _sut;

    public UnitTest1()
    { 
        _sut = new ServiceCollection()
            .RegisterLexicalParser()
            .BuildServiceProvider()
            .GetService<ILexicalParser>();
    }
    
    [Theory]
    [InlineData("( + 1 2 )")]
    [InlineData("(+ 1 2 )")]
    [InlineData("( + 1 2)")]
    [InlineData("(+ 1 2)")]
    public void VerifyOpeningBracketParsed(string input)
    {
        var result = _sut.Parse(input);

        Assert.IsType<OpeningBracketLexem>(result.First());
    }
    
    [Theory]
    [InlineData("( + 1 2 )")]
    [InlineData("(+ 1 2 )")]
    [InlineData("( + 1 2)")]
    [InlineData("(+ 1 2)")]
    public void VerifyClosingBracketParsed(string input)
    {
        var result = _sut.Parse(input);

        Assert.IsType<ClosingBracketLexem>(result.Last());
    }

    [Theory]
    [InlineData("( + A B )")]
    [InlineData("( + AA B )")]
    [InlineData("( + AA BB )")]
    [InlineData("( CC AA B )")]
    [InlineData("( CC AA B ))")]
    [InlineData("( define AA B ))")]


    public void VerifyParsingStringLiteral_ThreeLiterals(string input)
    {
        var result = _sut.Parse(input);

        Assert.IsType<StringLexem>(result[1]);
        Assert.IsType<StringLexem>(result[2]);
        Assert.IsType<StringLexem>(result[3]);
    }
    
    [Theory]
    [InlineData("( + A 1)")]
    [InlineData("( + ABCDEF 1)")]
    [InlineData("( + ABCDEF ))")]
    [InlineData("(define define)")]

    public void VerifyParsingStringLiteral_TwoLiterals(string input)
    {
        var result = _sut.Parse(input);

        Assert.IsType<StringLexem>(result[1]);
        Assert.IsType<StringLexem>(result[2]);
    }
    
    [Theory]
    [InlineData("( + 1 )", 1)]
    [InlineData("( + 13 )", 13)]
    [InlineData("( 2 1 ))", 1)]
    [InlineData("(define 1)", 1)]
    [InlineData("(define 12)", 12)]
    // [InlineData("(define A 21)", 1)]
    public void VerifyParsingNumberLiteral_ThirdLexem(string input, int expectedValueOf3RdLexem)
    {
        var result = _sut.Parse(input);

        var numberLiteral = result[2] as NumberLexem;

        Assert.Equal(expectedValueOf3RdLexem, numberLiteral!.Value);
    }
    
    [Theory]
    [InlineData("( + 1 4 )", 4)]
    [InlineData("( + 13 24 )", 24)]
    [InlineData("( 2 11 12))", 12)]
    [InlineData("(define 11 22)", 22)]
    [InlineData("(define A 300)", 300)]
    // [InlineData("(define A 21)", 1)]
    public void VerifyParsingNumberLiteral_FourthLexem(string input, int expectedValueOf4ThLexem)
    {
        var result = _sut.Parse(input);

        var numberLiteral = result[3] as NumberLexem;

        Assert.Equal(expectedValueOf4ThLexem, numberLiteral!.Value);
    }
}