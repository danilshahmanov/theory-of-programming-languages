using System.Text.RegularExpressions;
using TheoryOfProgrammingLanguages.Source.Exceptions;
using TheoryOfProgrammingLanguages.Source.Tokens;

namespace TheoryOfProgrammingLanguages.Source
{
    internal class Lexer
    {
        private string _input;
        private List<TokenDefinition> _tokenDefinitions;
        private int _cursorPosition;
        private Regex _regexIllegalToken;
        public Lexer(List<TokenDefinition> tokenDefinitions, string input)
        {
            _tokenDefinitions = tokenDefinitions;
            _input = input;
            _cursorPosition = 0;
            _regexIllegalToken = new Regex(@"^(.*?)\s");
        }
        private bool IsEndOfInput() => _cursorPosition >= _input.Length;
        public Queue<Token> GetTokens()
        {
            Queue<Token> tokens = new();
            while (!IsEndOfInput())
            {
                var token = GetNextToken();
                if (token != null)
                {
                    if (token.Type == TokenType.IDENTIFIER)
                        CheckLengthOfIdentifier((ValuedToken)token);
                    tokens.Enqueue(token);
                }
            }
            return tokens;
        }
        private void CheckLengthOfIdentifier(ValuedToken token)
        {
            if (token.Value.Length > 12)
                throw new IllegalTokenException($"Not correct identifier: {token}, max length of identifier is 12 symbols");
        }
        private Token? GetNextToken()
        {
            string target = _input.Substring(_cursorPosition);
            foreach (var tokenDefinition in _tokenDefinitions)
            {
                var matchResult = tokenDefinition.Reg.Match(target);
                if (!matchResult.Success)
                    continue;
                _cursorPosition += matchResult.Value.Length;
                if (tokenDefinition.IsIgnoredToken())
                    return null;
                if (tokenDefinition.IsValuedToken())
                    return new ValuedToken(tokenDefinition.Type, matchResult.Value);
                else
                    return new Token(tokenDefinition.Type);
            }
            //if there is no match to token
            throw new IllegalTokenException($"Illegal token at the input: {ParseIllegalToken(target)}");
        }
        private string ParseIllegalToken(string targetString)
        {
            var matchResult = _regexIllegalToken.Match(targetString);
            if (!matchResult.Success)
                return "Cannot parse illegal token";
            else
                return matchResult.Value;
        }
    }
}

