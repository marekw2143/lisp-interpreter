using LispInterpreter.AST.Expressions;
using LispInterpreter.Lexems;

namespace LispInterpreter.AST;

public interface IAbstractSyntaxTreeParser
{
    /// <summary>
    /// string literals are treated as variable names.
    /// <see cref="BaseExpression"/> implementations are responsible for parameter validations.
    ///
    /// this method DOES NOT parse lexems lines, which either define variable or define function,
    /// although it may be used to parse subset of such line.
    /// </summary>
    BaseExpression? ParseExpression(IReadOnlyList<Lexem> lexems, int idx, out int nextIndex, 
        IReadOnlyDictionary<string, FunctionEntry> entries);

    /// <summary>
    /// Tries to extract function definition from lexems line.
    /// </summary>
    bool TryExtractFuctionDefinition(IReadOnlyList<Lexem> lexems,
        out FunctionEntry functionEntry);

    /// <summary>
    /// Variable values may be only numbers
    /// (it's not possible to assign a value result of function expression).
    /// </summary>
    bool TryExtractVariableDefinition(IReadOnlyList<Lexem> lexems, out string name, out int value);
}