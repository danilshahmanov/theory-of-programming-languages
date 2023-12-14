using System.Text.RegularExpressions;

namespace TheoryOfProgrammingLanguages.Source.Tokens
{
    internal enum TokenType
    {
        //symbols, delimiters
        IGNORE,
        SEMICOLON,
        COLON,
        COMMA,
        OPEN_PARENTHESIS,
        CLOSE_PARENTHESIS,
        //literals
        NUMBER,
        //keywords
        BEGIN,
        VAR,
        END,
        INTEGER,
        READ,
        WRITE,
        FOR,
        TO,
        DO,
        END_FOR,
        //variables
        IDENTIFIER,
        //operators
        ADDITION_OPERATOR,
        SUBTRACTION_OPERATOR,
        MULTIPLICATION_OPERATOR,
        DIVISION_OPERATOR,
        NEGATE_OPERATOR,
        ASSIGNMENT_OPERATOR
    }
    internal class TokenDefinition
    {
        public Regex Reg { get; }
        public TokenType Type { get; }
        public TokenDefinition(string pattern, TokenType type)
        {
            Reg = new Regex(pattern);
            Type = type;
        }
        public bool IsValuedToken() => Type == TokenType.IDENTIFIER || Type == TokenType.NUMBER;
        public bool IsIgnoredToken() => Type == TokenType.IGNORE;
    }
}
