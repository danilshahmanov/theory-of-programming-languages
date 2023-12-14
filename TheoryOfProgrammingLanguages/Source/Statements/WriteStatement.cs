using TheoryOfProgrammingLanguages.Source.Tokens;

namespace TheoryOfProgrammingLanguages.Source.Statements
{
    internal class WriteStatement : IStatement
    {
        public StatementType Type { get; }
        public List<ValuedToken> Variables { get; }
        public WriteStatement(List<ValuedToken> variables)
        {
            Type = StatementType.WRITE_STATEMENT;
            Variables = variables;
        }
    }
}
