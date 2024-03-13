using System.Collections.Immutable;

namespace LispInterpreter.AST.Expressions;

/// <summary>
/// Represent's call of user defined function.
/// Something like "thunk" in haskell's world.
/// </summary>
sealed class UserFunctionExpression : BaseExpression
{
    private readonly FunctionEntry _functionEntry;
    private readonly IReadOnlyDictionary<string, BaseExpression> _actualCallParameters;
    
    private BaseExpression ExpressionBody => _functionEntry.FunctionBodyExpression;

    public UserFunctionExpression(FunctionEntry functionEntry, IReadOnlyDictionary<string, BaseExpression> actualCallParameters)
    {
        if (actualCallParameters.Count != functionEntry.ParametersCount)
        {
            throw new SyntaxException(
                $"Function {functionEntry.FunctionName} expects {functionEntry.ParametersCount} parameters, " +
                $"while called with {actualCallParameters.Count} parameters.");
        }
        
        _functionEntry = functionEntry;
        _actualCallParameters = actualCallParameters;
    }

    public override int Evaluate(IReadOnlyDictionary<string, int> variables)
    {
        var evaluatedCallParameters = _actualCallParameters
            .ToDictionary(x => x.Key,
                x => x.Value.Evaluate(variables));

        // function may use global variables
        // but it's (that is function's) parameters override global variables 
        foreach (var (k, v) in variables)
        {
            evaluatedCallParameters.TryAdd(k, v);
        }

        return ExpressionBody.Evaluate(evaluatedCallParameters.ToImmutableDictionary());
    }
}