using TheoryOfProgrammingLanguages.Source.Exceptions;

namespace TheoryOfProgrammingLanguages.Source.Tokens
{
    internal class Token : IComparable<Token>
    {
        public TokenType Type { get; }
        public Token(TokenType type) => Type = type;
        override
        public string ToString() => $"type: {Type}";
        //supposed to work only with operators tokens +, -, /, *, =
        public int CompareTo(Token other)
        {
            if (!IsOperator(other))
                throw new UnexpectedTokenException("Impossible to compare tokens. Only operators can be compared");
            switch (Type)
            {
                case TokenType.ASSIGNMENT_OPERATOR:
                    return 1;
                case TokenType.NEGATE_OPERATOR:
                    if (other.Type == TokenType.ASSIGNMENT_OPERATOR)
                        return -1;
                    if (other.Type == TokenType.NEGATE_OPERATOR)
                        return 0;
                    return 1;
                case TokenType.DIVISION_OPERATOR:
                case TokenType.MULTIPLICATION_OPERATOR:
                    if (other.Type == TokenType.DIVISION_OPERATOR || other.Type == TokenType.MULTIPLICATION_OPERATOR)
                        return 0;
                    if (other.Type == TokenType.NEGATE_OPERATOR || other.Type == TokenType.ASSIGNMENT_OPERATOR)
                        return -1;
                    return 1;
                case TokenType.ADDITION_OPERATOR:
                case TokenType.SUBTRACTION_OPERATOR:
                    if (other.Type == TokenType.ADDITION_OPERATOR || other.Type == TokenType.SUBTRACTION_OPERATOR)
                        return 0;
                    return -1;
                default:
                    throw new UnexpectedTokenException("Impossible to compare tokens. Only operators can be compared");
            }
        }
        public static bool IsOperator(Token token)
        {
            return token.Type == TokenType.ASSIGNMENT_OPERATOR ||
                token.Type == TokenType.ADDITION_OPERATOR ||
                token.Type == TokenType.SUBTRACTION_OPERATOR ||
                token.Type == TokenType.DIVISION_OPERATOR ||
                token.Type == TokenType.MULTIPLICATION_OPERATOR ||
                token.Type == TokenType.NEGATE_OPERATOR;
        }
    }
}
