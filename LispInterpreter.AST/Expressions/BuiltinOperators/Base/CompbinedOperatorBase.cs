namespace LispInterpreter.AST.Expressions.BuiltinOperators.Base;

/// <param name="func">Operation should be composable</param> 
abstract class CompbinedOperatorBase(Func<int, int, int> func, IReadOnlyList<BaseExpression> operandsList)
    : ParametrizedOperatorExpressionBase(operandsList)
{
    protected override void ValidateOperands(IReadOnlyList<BaseExpression> operands)
    {
    }

    protected override int DoEvaluate(IReadOnlyList<BaseExpression> operands, IReadOnlyDictionary<string, int> variables)
        => operands.Select(op => op.Evaluate(variables)).Aggregate(func);

}