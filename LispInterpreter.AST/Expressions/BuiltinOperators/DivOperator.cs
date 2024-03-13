using LispInterpreter.AST.Expressions.BuiltinOperators.Base;

namespace LispInterpreter.AST.Expressions.BuiltinOperators;

sealed class DivOperator(IReadOnlyList<BaseExpression> operandsList) 
    : TwoOperandsOperatorBase(operandsList, (x, y) => x / y);