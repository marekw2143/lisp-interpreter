namespace LispInterpreter.AST.Expressions.BuiltinOperators.Base;

abstract class TwoOperandsOperatorBase(IReadOnlyList<BaseExpression> operandsList, Func<int, int, int> func)
    : SpecificParametersCountOperatorBase(operandsList, 2)
{
    protected override int DoEvaluate(IReadOnlyList<BaseExpression> operands,
        IReadOnlyDictionary<string, int> variables)
    {
        var op1 = operands[0].Evaluate(variables);
        var op2 = operands[1].Evaluate(variables);

        return func(op1, op2);
    }
}