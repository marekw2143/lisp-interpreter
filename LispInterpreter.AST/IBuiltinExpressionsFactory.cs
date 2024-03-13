using LispInterpreter.AST.Expressions;

namespace LispInterpreter.AST;

interface IBuiltinExpressionsFactory
{
    BaseExpression BuildExpression(string operatorName,
        IReadOnlyList<BaseExpression> operands);
}