namespace LispInterpreter.AST.Expressions;
/// <summary>
/// Represent value of number literal.
/// Evaluation does not use any variables.
/// </summary>
sealed class NumberExpression : BaseExpression
{
    private readonly int _value;

    public NumberExpression(int value)
    {
        _value = value;
    }


    public override int Evaluate(IReadOnlyDictionary<string, int> _)
        => _value;
}