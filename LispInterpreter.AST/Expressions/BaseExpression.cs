namespace LispInterpreter.AST.Expressions;

public abstract class BaseExpression
{
    public abstract int Evaluate(IReadOnlyDictionary<string, int> variables);
}