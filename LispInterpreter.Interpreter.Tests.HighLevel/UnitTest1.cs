using Microsoft.Extensions.DependencyInjection;

namespace LispInterpreter.Interpreter.Tests.HighLevel;

public class UnitTest1
{
    private readonly Interpreter _sut;

    public UnitTest1()
    {

        var services = new ServiceCollection();

        services.RegisterInterpreter();

        var provider = services.BuildServiceProvider();

        _sut = provider.GetService<Interpreter>();
    }

    [Fact]
    public void DefiningTwoVariables_AndCallingBuiltinOperator()
    {
        string[] input = new[]
        {
            "( define A 15 )",
            "(define B 30 )",
            "( + A B )"
        };

        var results = _sut.ProcessInputs(input);

        Assert.Equal(45, results.Values.Single());
    }

    [Fact]
    public void CallingFunction_WithVariablesAndNumberLiterals_AsParameters()
    {
        string[] input = new[]
        {
            "( define A 15 )",
            "(define B 30 )",
            "(define (ABC V X Y Z) ( + V (+ A (* Y Z ) ) ) )",
            "( ABC 1 A 3 B )" // should be: 1 + (15 + (3 * 30) ) => 106
        };

        var results = _sut.ProcessInputs(input);

        Assert.Equal(106, results.Values.Single());
    }

    [Fact]
    public void ConditionalStatements_Positive()
    {
        string[] input = new[]
        {
            "(define DWA 2)",
            "(define TRZY 3)",
            "(define CZTERY 4 )",
            "( if (> 3 1) (+ 2 7) (+ 9 10 CZTERY DWA ) )",
        };

        var results = _sut.ProcessInputs(input);

        Assert.Equal(9, results.Values.Single());
    }
    
    [Fact]
    public void ConditionalStatements_Negative()
    {
        string[] input = new[]
        {
            "(define DWA 2)",
            "(define TRZY 3)",
            "(define CZTERY 4 )",
            "( if (= 3 1) (+ 2 7) (+ 9 10 CZTERY DWA ) )",
        };

        var results = _sut.ProcessInputs(input);

        Assert.Equal(25, results.Values.Single());
    }
}