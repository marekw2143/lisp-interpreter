using System.Collections.Immutable;
using System.Text;

namespace LispInterpreter.Lexems;

class LexicalParser(LexerOptions options) : ILexicalParser
{

    public ImmutableList<Lexem> Parse(string input)
    {
        input = input.Trim(); // input data preprocessing
    
        var ret = new List<Lexem>();

        void addLexem(Lexem toAdd)
            => ret.Add(toAdd);

        var idx = 0;
        while (true)
        {
            if (idx == input.Length)
                break;

            var c = input[idx++];

            bool equalsChar(char compared)
                => c == compared;


            string readNextStringIncluddingCurrentChar()
            {
                bool endOfStringLiteral()
                    => !char.IsAsciiLetterOrDigit(c);
                
                var sb = new StringBuilder();

                sb.Append(c);
            
                while (true)
                {

                    c = input[idx];

                    if (endOfStringLiteral())
                    {
                        return sb.ToString();
                    }

                    sb.Append(c);
                    idx++;
                }
            }
        
            if (equalsChar(options.LeftBracketCharacter))
            {
                addLexem(new OpeningBracketLexem());

                continue;
            }

            if (equalsChar(options.RightBracketCharacter))
            {
                addLexem(new ClosingBracketLexem());

                continue;
            }

            if (equalsChar(' '))
            {
                continue;
            }
        
            if (char.IsDigit(c))
            {
                var stringRepr = readNextStringIncluddingCurrentChar();

                var numberValue = int.Parse(stringRepr);

                addLexem(new NumberLexem(numberValue));
                continue;
            }
        
            if (char.IsAscii(c))
            {
                addLexem(new StringLexem(readNextStringIncluddingCurrentChar()));

                continue;
            }
        }

        return ret.ToImmutableList();
    }
}