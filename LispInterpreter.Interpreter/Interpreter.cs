using System.Collections.Frozen;
using System.Collections.Immutable;
using LispInterpreter.AST;
using LispInterpreter.Lexems;

namespace LispInterpreter.Interpreter;

/// <summary>
/// Algorithm is as follows: 
/// 1. parse each line into list of lexems
/// 2. parse each list of lexems, and 
/// 2. a. if it contains varialbe definition -> store information in "variables" 
/// 2. b. if it contains function definition -> store function definition (AKA FunctionEntry) in "functionEntries" 
/// 2. c. otherwise, if line neither defines variable nor function -> store it int "lexemLinesToEvaluate" , 
///       which stores lines of lexems of program code without variable and function definitions
/// 3. Evaluate result of each "line" of lexems" from "lexemLinesToEvaluate" and print the result 
/// </summary>
/// <param name="astParser"></param>
/// <param name="lexParser"></param>
public class Interpreter(IAbstractSyntaxTreeParser astParser, ILexicalParser lexParser)
{
    public class LineDescriptor(int lineNumber, string lineContent)
    {
        public int LineNumber { get; init; } = lineNumber;
        public string LineContent { get; init; } = lineContent;
    }
    
    /// <returns>mapping of input line code descriptor and that line's evaluation result.
    /// Could not simply return dictionary<string, int>, as <see cref="inputLines"/>
    /// may contain duplicated lines (although result for them should be the same).
    /// </returns>
    public Dictionary<LineDescriptor, int> ProcessInputs(string[] inputLines)
    {
        var lexemLines = inputLines
            .Select(lexParser.Parse)
            .ToImmutableList();

        var variables = new Dictionary<string, int>();

        var lexemLinesToEvaluate = new Dictionary<ImmutableList<Lexem>, LineDescriptor>();

        var functionEntries = new Dictionary<string, FunctionEntry>();

        var index = 0;
        foreach (var (lexemLine, inputLine) in lexemLines.Zip(inputLines))
        {
            index++;
            if (astParser.TryExtractVariableDefinition(lexemLine, out var name, out var value))
            {
                variables.Add(name, value);
            }
            else if (astParser.TryExtractFuctionDefinition(lexemLine, out var functionEntry))
            {
                functionEntries[functionEntry.FunctionName] = functionEntry;
            }
            else
            {
                lexemLinesToEvaluate.Add(lexemLine, new(index, inputLine));
            }
        }

        var variablesFrozen = variables.ToFrozenDictionary(StringComparer.Ordinal);

        var ret = new Dictionary<LineDescriptor, int>(lexemLinesToEvaluate.Count);

        foreach (var (lexemLine, inputLine) in lexemLinesToEvaluate)
        {
            var tree = astParser.ParseExpression(lexemLine, 0, out var _, functionEntries);

            var result = tree.Evaluate(variablesFrozen);

            ret[inputLine] = result;
        }

        return ret;
    }
}