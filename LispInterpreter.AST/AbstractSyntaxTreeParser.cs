using System.Collections.Immutable;
using LispInterpreter.AST.Expressions;
using LispInterpreter.AST.Expressions.BuiltinOperators;
using LispInterpreter.Lexems;

namespace LispInterpreter.AST;

class AbstractSyntaxTreeParser(IBuiltinExpressionsFactory builtinExpressionsFactory) : IAbstractSyntaxTreeParser
{

    public BaseExpression? ParseSimpleExpression(IReadOnlyList<Lexem> lexems, int idx, out int nextIndex,
        IReadOnlyDictionary<string, FunctionEntry> entries)
    {
        while (true)
        {
            Lexem NextLexem()
                => NextNthLexem(1);
            
            Lexem NextNthLexem(int nextNumber)
                => lexems[idx + nextNumber];
            
            var current = lexems[idx];

            if (current is OpeningBracketLexem)
            {
                if (NextLexem() is StringLexem lFunctionName) // function name
                {
                    var operands = new List<BaseExpression>();

                    var operandIndex = idx + 2;

                    while (true)
                    {
                        var operand = ParseExpression(lexems,
                            operandIndex,
                            out var nextLexemIndex,
                            entries);

                        if (operand is null) // detected ')', so 'nextLexemIndex' points to next lexem after ')'
                        {
                            nextIndex = nextLexemIndex;
                            
                            break;
                        }
                        
                        operands.Add(operand);

                        operandIndex = nextLexemIndex;
                    }

                    

                    var functionName = lFunctionName.Value;
                    
                    // it's function from user-defined function list list
                    if (entries.TryGetValue(functionName, out var functionEntry)) 
                    {
                        var callParameters = functionEntry.OrderedParemeterNames
                            .Zip(operands)
                            .ToDictionary()
                            .ToImmutableDictionary();
                        
                        return new UserFunctionExpression(functionEntry, callParameters);
                    }
                    else // otherwise, it's built-in function
                    {
                       return builtinExpressionsFactory.BuildExpression(functionName, operands);
                    }
                }
            }

            if (current is NumberLexem numberLexem) // simply number literal
            {
                nextIndex = idx + 1;

                return new NumberExpression(numberLexem.Value);
            }
            
            if (current is StringLexem stringLexem)
            {
                var variableName = stringLexem.Value;
               
                nextIndex = idx + 1;
                
                return new VariableValueExpression(variableName);
            }

            if (current is ClosingBracketLexem)
            {
                nextIndex = idx + 1;

                return null;
            }

            throw new SyntaxException($"Cannot parse current node {current}");
        }
    }
    /// <summary>
    /// string literals are treated as variable names.
    /// <see cref="BaseExpression"/> implementations are responsible for parameter validations.
    /// </summary>
    public BaseExpression? ParseExpression(IReadOnlyList<Lexem> lexems, int idx, out int nextIndex, 
        IReadOnlyDictionary<string, FunctionEntry> entries)
    {
        if (IsConditionalStatement(lexems))
        {
            return ParseIfExpression(lexems, out nextIndex, entries);
        }

        return ParseSimpleExpression(lexems, idx, out nextIndex, entries);
    }

    private BaseExpression? ParseIfExpression(IReadOnlyList<Lexem> lexems, out int nextIndex, IReadOnlyDictionary<string, FunctionEntry> entries)
    {
        BaseExpression buildLiteralOrVariableExpr(Lexem lexem)
            => lexem switch
            {
                NumberLexem numberLexem => new NumberExpression(numberLexem.Value),
                StringLexem stringLexem => new VariableValueExpression(stringLexem.Value),
                _ => throw new SyntaxException("In conditional operator must provide either numeric value or variable")
            };
        
       var branch1 = ParseSimpleExpression(lexems.Skip(7).ToImmutableList(),
                0,
                out nextIndex, entries); //get starting index of next expression

        var branch2 =  ParseSimpleExpression(lexems.Skip(7 + nextIndex).ToImmutableList(),
                0, 
                out nextIndex,
                entries);

        return new ConditionalOperator()
        {
            BranchIfPredicateMatches = branch1,
            BranchIfPredicateDoNotMatch = branch2,
            Operand1 = buildLiteralOrVariableExpr(lexems[4]),
            Operand2 = buildLiteralOrVariableExpr(lexems[5]),
            ComparisionOperator = (lexems[3] as StringLexem).Value
        };
    }

    bool IsConditionalStatement(IReadOnlyList<Lexem> lexems)
        =>
            lexems.First() is OpeningBracketLexem
            && IsIfKeywordLexem(lexems[1])
            && lexems[2] is OpeningBracketLexem
            && lexems.Last() is ClosingBracketLexem;
    
    public bool TryExtractFuctionDefinition(IReadOnlyList<Lexem> lexems,
        out FunctionEntry functionEntry)
    {
        //
        // check if it's user defined function.
        //
        // check if we have:
        // "( define ( "
        // prefix matched
        // AND last lexem represents closing parenthesis ')'
        //
        if (lexems.First() is OpeningBracketLexem && lexems.Last() is ClosingBracketLexem
                                       && IsDefineKeywordLexem(lexems[1])
                                       && lexems[2] is OpeningBracketLexem) 
        {
            if (lexems[3] is StringLexem functionNameLexem)
            {
                var idx = 4;

                var paramNames = new List<string>();
                while (true)
                {
                    var curr = lexems[idx];

                    if (curr is StringLexem paramNameLexem)
                    {
                        // if (functionEntries.TryGetValue(paramNameLexem.Value, out var entry))
                        // {
                        //     
                        // }
                        // else
                        // {
                        paramNames.Add(paramNameLexem.Value);
                        idx++;
                        continue;
                        // }
                    }
                    
                    if (curr is ClosingBracketLexem) // end of argument declarations
                    {
                        idx++;
                        break;
                    }

                    // if (curr is OpeningBracketLexem) // inner expression
                    // {
                    //     var parameter = ParseExpression(lexems.Skip(idx).ToImmutableList(),
                    //         0, out var newIdx2,
                    //         new Dictionary<string, FunctionEntry>());
                    //     
                    //     
                    // }

                    throw new Exception();
                }

                var functionBodyExpression = 
                    ParseExpression(lexems.Skip(idx).ToImmutableList(), 0, out var i2, new Dictionary<string, FunctionEntry>());

                functionEntry = new FunctionEntry()
                {
                    FunctionName = functionNameLexem.Value,
                    FunctionBodyExpression = functionBodyExpression,
                    OrderedParemeterNames = paramNames.ToImmutableList()
                };
                
                return true;
            }
        }
        
        functionEntry = null;
        return false;
    }
    
    /// <summary>
    /// Variable values may be only numbers (it's not possible to assing a value result of function expression).
    /// </summary>
    /// <param name="lexems"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryExtractVariableDefinition(IReadOnlyList<Lexem> lexems, out string name, out int value)
    {
        if (lexems.First() is OpeningBracketLexem
            && lexems.Last() is ClosingBracketLexem)
        {
            var maybeVariableNameLexem = lexems[2] as StringLexem;
            var maybeVariableValueLexem = lexems[3] as NumberLexem;

            if (IsDefineKeywordLexem(lexems[1])
                && maybeVariableValueLexem is not null)
            {
                value = maybeVariableValueLexem.Value;
                name = maybeVariableNameLexem.Value;

                return true;
            }
        }

        value = int.MinValue;
        name = string.Empty;
        return false;
    }
    
    private bool IsDefineKeywordLexem(Lexem lexem)
    {
        if (lexem is StringLexem stringLexem)
        {
            return stringLexem.Value == "define";
        }

        return false;
    }
    
    private bool IsIfKeywordLexem(Lexem lexem)
    {
        if (lexem is StringLexem stringLexem)
        {
            return stringLexem.Value == "if";
        }

        return false;
    }
}