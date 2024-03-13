namespace LispInterpreter.AST.Expressions;

/// <summary>
/// Variable value
/// </summary>
sealed class VariableValueExpression : BaseExpression
{
    private readonly string _variableName;
    public VariableValueExpression(string variableName)
    {
        _variableName = variableName;
    }

    public override int Evaluate(IReadOnlyDictionary<string, int> variables)
    {
        return variables[_variableName];
    }
}