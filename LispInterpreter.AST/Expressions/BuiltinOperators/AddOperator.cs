using LispInterpreter.AST.Expressions.BuiltinOperators.Base;

namespace LispInterpreter.AST.Expressions.BuiltinOperators;

sealed class AddOperator(IReadOnlyList<BaseExpression> operandsList) 
    : CompbinedOperatorBase((x, y) => x + y, operandsList);