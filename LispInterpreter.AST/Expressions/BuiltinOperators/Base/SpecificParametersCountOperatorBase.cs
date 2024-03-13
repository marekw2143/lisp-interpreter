namespace LispInterpreter.AST.Expressions.BuiltinOperators.Base;


/// <summary>
/// Provides validation of number of parameters provided.
/// </summary>
abstract class SpecificParametersCountOperatorBase(
    IReadOnlyList<BaseExpression> operandsList,
    int requiredNumberOfParameters)
    : ParametrizedOperatorExpressionBase(operandsList)
{
    protected override void ValidateOperands(IReadOnlyList<BaseExpression> operands)
    {
        if (operands.Count != requiredNumberOfParameters)
        {
            throw new SyntaxException($"This operator requires {requiredNumberOfParameters} parameters, " +
                                      $"while provided {operands.Count} parameters");
        }
    }
}