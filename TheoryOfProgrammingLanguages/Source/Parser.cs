
using TheoryOfProgrammingLanguages.Source.Statements;
using TheoryOfProgrammingLanguages.Source.Exceptions;
using TheoryOfProgrammingLanguages.Source.Tokens;
using TheoryOfProgrammingLanguages.Source.Expressions;

namespace TheoryOfProgrammingLanguages.Source
{
    internal class Parser
    {
        private Queue<Token> _tokens;
        public Parser(Queue<Token> tokens) => _tokens = tokens;
        private bool HasTokens() => _tokens.Count > 0;
        private Token ConsumeExpectedCurrentToken(TokenType type)
        {
            if (!HasTokens())
                throw new UnexpectedEndOfProgramException($"Unexpected end of program, expected next token with type: \"{type}\"");
            var token = _tokens.Dequeue();
            if (token.Type != type)
                throw new UnexpectedTokenException($"Unexpected token: \"{token}\", expected token with type: \"{type}\"");
            return token;
        }
        private bool TryConsumeExpectedCurrentToken(TokenType type)
        {
            if (!HasTokens())
                throw new UnexpectedEndOfProgramException($"Unexpected end of program, expected next token with type: \"{type}\"");
            if (_tokens.Peek().Type == type)
            {
                _tokens.Dequeue();
                return true;
            }
            else
                return false;
        }
        private Token PeekCurrentToken()
        {
            if (!HasTokens())
                throw new UnexpectedEndOfProgramException($"Unexpected end of program, expected existing next token");
            return _tokens.Peek();
        }
        public void ParseProgram(out List<ValuedToken> variables, out List<IStatement> statements)
        {
            variables = ParseDeclaration();
            ConsumeExpectedCurrentToken(TokenType.BEGIN);
            statements = ParseInternalStatements(TokenType.END);
            ConsumeExpectedCurrentToken(TokenType.END);
            if (HasTokens())
                throw new UnexpectedEndOfProgramException($"Unexpected end of program. There is symbol after {TokenType.END}: \"{PeekCurrentToken()}\"");
        }
        private List<ValuedToken> ParseDeclaration()
        {
            ConsumeExpectedCurrentToken(TokenType.VAR);
            var variables = GetVariablesList();
            ConsumeExpectedCurrentToken(TokenType.COLON);
            ConsumeExpectedCurrentToken(TokenType.INTEGER);
            ConsumeExpectedCurrentToken(TokenType.SEMICOLON);
            return variables;
        }
        private List<ValuedToken> GetVariablesList()
        {
            List<ValuedToken> variables = new();
            do
                variables.Add((ValuedToken)ConsumeExpectedCurrentToken(TokenType.IDENTIFIER));
            while (TryConsumeExpectedCurrentToken(TokenType.COMMA));
            return variables;
        }

        private List<IStatement> ParseInternalStatements(TokenType endTokenType)
        {
            List<IStatement> statements = new List<IStatement>();
            while (PeekCurrentToken().Type != endTokenType)
                statements.Add(GetNextStatement());
            return statements;
        }
        private IStatement GetNextStatement()
        {
            switch (PeekCurrentToken().Type)
            {
                case TokenType.READ:
                    return ParseReadStatement();

                case TokenType.WRITE:
                    return ParseWriteStatement();

                case TokenType.FOR:
                    return ParseForStatement();

                case TokenType.IDENTIFIER:
                    return ParseAssignmentStatement();

                default:
                    throw new UnexpectedTokenException($"Unexpected token during executing statements: \"{PeekCurrentToken()}\"");
            }
        }
        private ForStatement ParseForStatement()
        {
            ConsumeExpectedCurrentToken(TokenType.FOR);

            var iteratorVariable = (ValuedToken)ConsumeExpectedCurrentToken(TokenType.IDENTIFIER);
            ConsumeExpectedCurrentToken(TokenType.ASSIGNMENT_OPERATOR);
            var startIteratorValueExpression = new List<Token>();
            GetInfixExpression(startIteratorValueExpression, TokenType.TO);
            ConsumeExpectedCurrentToken(TokenType.TO);

            List<Token> endIteratorValueInfixExpression = new();
            GetInfixExpression(endIteratorValueInfixExpression, TokenType.DO);
            ConsumeExpectedCurrentToken(TokenType.DO);

            var statements = ParseInternalStatements(TokenType.END_FOR);
            ConsumeExpectedCurrentToken(TokenType.END_FOR);

            return new ForStatement(iteratorVariable, startIteratorValueExpression, endIteratorValueInfixExpression, statements);
        }
        private ReadStatement ParseReadStatement()
        {
            ConsumeExpectedCurrentToken(TokenType.READ);
            ConsumeExpectedCurrentToken(TokenType.OPEN_PARENTHESIS);
            ReadStatement readStatement = new(GetVariablesList());
            ConsumeExpectedCurrentToken(TokenType.CLOSE_PARENTHESIS);
            ConsumeExpectedCurrentToken(TokenType.SEMICOLON);
            return readStatement;
        }
        private WriteStatement ParseWriteStatement()
        {
            ConsumeExpectedCurrentToken(TokenType.WRITE);
            ConsumeExpectedCurrentToken(TokenType.OPEN_PARENTHESIS);
            WriteStatement writeStatement = new(GetVariablesList());
            ConsumeExpectedCurrentToken(TokenType.CLOSE_PARENTHESIS);
            ConsumeExpectedCurrentToken(TokenType.SEMICOLON);
            return writeStatement;
        }
        private AssignmentStatement ParseAssignmentStatement()
        {
            var variable = (ValuedToken)ConsumeExpectedCurrentToken(TokenType.IDENTIFIER);
            ConsumeExpectedCurrentToken(TokenType.ASSIGNMENT_OPERATOR);
            List<Token> valueExpression = new();
            GetInfixExpression(valueExpression, TokenType.SEMICOLON);
            ConsumeExpectedCurrentToken(TokenType.SEMICOLON);
            return new AssignmentStatement(variable, valueExpression);
        }
        private void GetInfixExpression(List<Token> infixExpression, TokenType endTokenType)
        {
            Token? prevExpressionToken = null;
            while (PeekCurrentToken().Type != endTokenType)
            {
                var currentExpressionToken = ConsumeExpectedCurrentToken(PeekCurrentToken().Type);
                if (currentExpressionToken.Type == TokenType.OPEN_PARENTHESIS)
                {
                    infixExpression.Add(currentExpressionToken);
                    GetInfixExpression(infixExpression, TokenType.CLOSE_PARENTHESIS);
                    prevExpressionToken = ConsumeExpectedCurrentToken(TokenType.CLOSE_PARENTHESIS);
                    infixExpression.Add(prevExpressionToken);
                    continue;
                }
                if (ExpressionHelper.IsTokenValidToInfixExpression(prevExpressionToken, currentExpressionToken))
                {
                    prevExpressionToken = currentExpressionToken;
                    infixExpression.Add(prevExpressionToken);
                }
                else
                    throw new UnexpectedTokenException($"Unexpected token in the expression: \"{currentExpressionToken}\"");
            }
            if (!ExpressionHelper.IsEndOfInfixExpressionValid(prevExpressionToken))
                throw new Exception($"Unexpected end of expression/internal expression \"{prevExpressionToken}\" after \"{PeekCurrentToken()}\"");
        }
    }
}


