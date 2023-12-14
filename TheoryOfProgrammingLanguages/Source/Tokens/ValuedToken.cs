
namespace TheoryOfProgrammingLanguages.Source.Tokens
{
    internal class ValuedToken : Token
    {
        public string Value { get; }
        public ValuedToken(TokenType type, string value) : base(type) => Value = value;
        override
        public string ToString() => $"type: {Type}, value: {Value}";
    }
}
