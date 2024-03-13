using System.Collections.Immutable;
using LispInterpreter.AST.Expressions;

namespace LispInterpreter.AST;

public class FunctionEntry
{
    public int ParametersCount
        => OrderedParemeterNames.Count;

    public BaseExpression FunctionBodyExpression { get; init; }
    public ImmutableList<string> OrderedParemeterNames{ get; init; }
    public string FunctionName { get; init; }
}