namespace LispInterpreter.AST.Expressions.BuiltinOperators.Base;


/// <remarks>subclasses need to implement both <see cref="ValidateOperands"/> and <see cref="DoEvaluate"/>.</remarks>
abstract class ParametrizedOperatorExpressionBase(IReadOnlyList<BaseExpression> operandsList) : BaseExpression
{
    protected abstract void ValidateOperands(IReadOnlyList<BaseExpression> operands);

    protected abstract int DoEvaluate(IReadOnlyList<BaseExpression> operands, IReadOnlyDictionary<string, int> variables);

    /// <remarks>Template method</remarks>
    public override int Evaluate(IReadOnlyDictionary<string, int> variables)
    {
        ValidateOperands(operandsList);

        return DoEvaluate(operandsList, variables);
    }
}