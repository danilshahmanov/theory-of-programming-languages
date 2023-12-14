using TheoryOfProgrammingLanguages.Source.Exceptions;
using TheoryOfProgrammingLanguages.Source.Tokens;

namespace TheoryOfProgrammingLanguages.Source.Expressions
{
    internal class ExpressionHelper
    {

        public delegate int GetVariableValue(ValuedToken variable);

        private static bool CanAddBinaryOperatorToInfixExpression(Token? prevToken)
        {
            //allowed after IDENTIFIER, NUMBER, )
            return prevToken != null &&
                (prevToken.Type == TokenType.IDENTIFIER ||
                prevToken.Type == TokenType.NUMBER ||
                prevToken.Type == TokenType.CLOSE_PARENTHESIS);
        }
        private static bool CanAddUnaryOperatorToInfixExpression(Token? prevToken)
        {
            //allowed in the begin and after +, -, /, *, (
            return prevToken == null ||
                prevToken.Type == TokenType.ADDITION_OPERATOR ||
                prevToken.Type == TokenType.SUBTRACTION_OPERATOR ||
                prevToken.Type == TokenType.MULTIPLICATION_OPERATOR ||
                prevToken.Type == TokenType.DIVISION_OPERATOR ||
                prevToken.Type == TokenType.OPEN_PARENTHESIS;
        }
        private static bool CanAddOperandToInfixExpression(Token? prevToken)
        {
            //allowed in the begin and after +, -, /, *, (, !
            return prevToken == null ||
                (prevToken.Type != TokenType.IDENTIFIER &&
                prevToken.Type != TokenType.NUMBER &&
                prevToken.Type != TokenType.CLOSE_PARENTHESIS);
        }
        public static int ParseToInt(string value) => int.Parse(value);
        public static bool IsEndOfInfixExpressionValid(Token? endToken)
        {
            //allowed end with IDENTIFIER, NUMBER, (
            return endToken != null &&
                (endToken.Type == TokenType.IDENTIFIER ||
                endToken.Type == TokenType.NUMBER ||
                endToken.Type == TokenType.CLOSE_PARENTHESIS);
        }
        public static bool IsTokenValidToInfixExpression(Token? prevToken, Token currentToken)
        {
            switch (currentToken.Type)
            {
                case TokenType.IDENTIFIER:
                case TokenType.NUMBER:
                    return CanAddOperandToInfixExpression(prevToken);

                case TokenType.ADDITION_OPERATOR:
                case TokenType.SUBTRACTION_OPERATOR:
                case TokenType.DIVISION_OPERATOR:
                case TokenType.MULTIPLICATION_OPERATOR:
                    return CanAddBinaryOperatorToInfixExpression(prevToken);

                case TokenType.NEGATE_OPERATOR:
                    return CanAddUnaryOperatorToInfixExpression(prevToken);

                default:
                    throw new UnexpectedTokenException($"Unexpected token in the expression: \"{currentToken}\" after token \"{prevToken}\"");
            }
        }
        private static bool IsEndOfInternalExpressionOperators(Stack<Token> operators)
        {
            return operators.Count != 0 && operators.Peek().Type != TokenType.OPEN_PARENTHESIS;
        }
        private static List<Token> GetPostfixExpression(List<Token> infixExpression)
        {
            List<Token> postfixExpression = new();
            Stack<Token> operators = new();
            foreach (Token token in infixExpression)
            {
                if (Token.IsOperator(token))
                {
                    while (IsEndOfInternalExpressionOperators(operators) && operators.Peek().CompareTo(token) >= 0)
                        postfixExpression.Add(operators.Pop());
                    operators.Push(token);
                    continue;
                }
                switch (token.Type)
                {
                    case TokenType.NUMBER:
                    case TokenType.IDENTIFIER:
                        postfixExpression.Add(token);
                        break;

                    case TokenType.OPEN_PARENTHESIS:
                        operators.Push(token);
                        break;

                    case TokenType.CLOSE_PARENTHESIS:
                        while (IsEndOfInternalExpressionOperators(operators))
                            postfixExpression.Add(operators.Pop());
                        operators.Pop();
                        break;
                    default:
                        throw new UnexpectedTokenException($"Undefined token in the expression: {token}");
                }
            }
            while (operators.Count != 0)
                postfixExpression.Add(operators.Pop());
            return postfixExpression;
        }

        public static int EvaluateInfixExpressionWithVariables(List<Token> infixExpression, GetVariableValue getVariableValue)
        {
            Stack<int> operands = new();
            var postfixExpression = GetPostfixExpression(infixExpression);
            foreach (Token token in postfixExpression)
            {
                switch (token.Type)
                {
                    case TokenType.ADDITION_OPERATOR:
                    case TokenType.SUBTRACTION_OPERATOR:
                    case TokenType.MULTIPLICATION_OPERATOR:
                    case TokenType.DIVISION_OPERATOR:
                    case TokenType.NEGATE_OPERATOR:
                        operands.Push(GetResultOfCurrentOperation(token, operands));
                        break;

                    case TokenType.NUMBER:
                        operands.Push(ParseToInt(((ValuedToken)token).Value));
                        break;

                    case TokenType.IDENTIFIER:
                        operands.Push(getVariableValue((ValuedToken)token));
                        break;

                    default:
                        throw new UnexpectedTokenException($"Undefined token in the expression: {token}");
                }
            }
            return operands.Pop();
        }
        private static int GetResultOfCurrentOperation(Token operatorToken, Stack<int> operands)
        {
            int left;
            int right;
            switch (operatorToken.Type)
            {
                case TokenType.ADDITION_OPERATOR:
                    return operands.Pop() + operands.Pop();
                case TokenType.SUBTRACTION_OPERATOR:
                    right = operands.Pop();
                    left = operands.Pop();
                    return left - right;
                case TokenType.MULTIPLICATION_OPERATOR:
                    return operands.Pop() * operands.Pop();
                case TokenType.DIVISION_OPERATOR:
                    right = operands.Pop();
                    left = operands.Pop();
                    return left / right;
                case TokenType.NEGATE_OPERATOR:
                    return -operands.Pop();
                default:
                    throw new UnexpectedTokenException($"Undefined operator in the expression: {operatorToken}");
            }
        }
    }
}
