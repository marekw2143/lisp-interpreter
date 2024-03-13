namespace LispInterpreter.AST.Expressions.BuiltinOperators;

public class ConditionalOperator :BaseExpression
{
    public BaseExpression Operand1 { get; init; }
    public BaseExpression Operand2 { get; init; }
    public String ComparisionOperator { get; init; }

    public BaseExpression BranchIfPredicateMatches { get; init; }
    public BaseExpression BranchIfPredicateDoNotMatch { get; init; }

    public override int Evaluate(IReadOnlyDictionary<string, int> variables)
    {
        Func<int, int, bool> predicate = ComparisionOperator switch
        {
            ">" => (x, y) => x > y,
            // ">" => (x, y) => x >= y,
            "=" => (x, y) => x == y,
            // "<" => (x, y) => x <= y,
            "<" => (x, y) => x < y,
            _ => throw new SyntaxException("Invalid operand")
        };

        var operand1Value = Operand1.Evaluate(variables);
        var operand2Value = Operand2.Evaluate(variables);

        if (predicate(operand1Value, operand2Value))
        {
            return BranchIfPredicateMatches.Evaluate(variables);
        }
        else
        {
            return BranchIfPredicateDoNotMatch.Evaluate(variables);
        }
    }
}