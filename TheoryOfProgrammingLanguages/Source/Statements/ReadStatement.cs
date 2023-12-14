using TheoryOfProgrammingLanguages.Source.Tokens;

namespace TheoryOfProgrammingLanguages.Source.Statements
{
    internal class ReadStatement : IStatement
    {
        public StatementType Type { get; }
        public List<ValuedToken> Variables { get; }
        public ReadStatement(List<ValuedToken> variables)
        {
            Type = StatementType.READ_STATEMENT;
            Variables = variables;
        }
    }

}
