using TheoryOfProgrammingLanguages.Source.Tokens;

namespace TheoryOfProgrammingLanguages.Source.Statements
{
    internal class AssignmentStatement : IStatement
    {
        public StatementType Type { get; }
        public ValuedToken Variable { get; }
        public List<Token> ValueInfixExpression { get; }
        public AssignmentStatement(ValuedToken variable, List<Token> valueInfixExpression)
        {
            Type = StatementType.ASSIGNMENT_STATEMENT;
            Variable = variable;
            ValueInfixExpression = valueInfixExpression;
        }
    }
}
