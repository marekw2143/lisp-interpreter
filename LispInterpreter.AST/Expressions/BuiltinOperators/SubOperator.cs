using LispInterpreter.AST.Expressions.BuiltinOperators.Base;

namespace LispInterpreter.AST.Expressions.BuiltinOperators;

sealed class SubOperator(IReadOnlyList<BaseExpression> operandsList) 
    : TwoOperandsOperatorBase(operandsList, (x, y) => x - y);