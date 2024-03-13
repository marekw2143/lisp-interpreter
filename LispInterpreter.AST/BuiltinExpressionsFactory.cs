using LispInterpreter.AST.Expressions;
using LispInterpreter.AST.Expressions.BuiltinOperators;

namespace LispInterpreter.AST;

class BuiltinExpressionsFactory : IBuiltinExpressionsFactory
{
    public BaseExpression BuildExpression(string operatorName,
        IReadOnlyList<BaseExpression> operands)
        => operatorName switch
        {
            "+" => new AddOperator(operands),
            "-" => new SubOperator(operands),
            "*" => new MulOperator(operands),
            "/" => new DivOperator(operands),
            _ => throw new ArgumentOutOfRangeException(nameof(operatorName), operatorName, "Unknown operator")
        };
}