
namespace TheoryOfProgrammingLanguages.Source.Statements
{
    internal enum StatementType
    {
        READ_STATEMENT,
        WRITE_STATEMENT,
        FOR_STATEMENT,
        ASSIGNMENT_STATEMENT
    }
    internal interface IStatement
    {
        public StatementType Type { get; }
    }
}
