using TheoryOfProgrammingLanguages.Source.Tokens;

namespace TheoryOfProgrammingLanguages.Source.Statements
{
    internal class ForStatement : IStatement
    {
        public StatementType Type { get; }
        public AssignmentStatement IteratorStatement { get; }
        public List<Token> EndIteratorValueExpression { get; }
        public List<IStatement> Statements { get; }
        public ForStatement(ValuedToken iteratorVariable, List<Token> startIteratorValueExpression, List<Token> endIteratorValueExpression, List<IStatement> statements)
        {
            Type = StatementType.FOR_STATEMENT;
            IteratorStatement = new AssignmentStatement(iteratorVariable, startIteratorValueExpression);
            EndIteratorValueExpression = endIteratorValueExpression;
            Statements = statements;
        }
    }
}
