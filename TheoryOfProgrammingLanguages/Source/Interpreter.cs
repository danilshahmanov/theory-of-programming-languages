using TheoryOfProgrammingLanguages.Source.Statements;
using TheoryOfProgrammingLanguages.Source.Tokens;
using TheoryOfProgrammingLanguages.Source.Exceptions;
using TheoryOfProgrammingLanguages.Source.Expressions;

namespace TheoryOfProgrammingLanguages.Source
{

    internal class Interpreter
    {
        private Dictionary<string, int?> _variables;
        public Interpreter() => _variables = new Dictionary<string, int?>();
        private bool IsVariableDefined(ValuedToken variable) => _variables.ContainsKey(variable.Value);
        private bool IsVariableAssigned(ValuedToken variable) => _variables[variable.Value] != null;
        private void DefineVariable(ValuedToken variable)
        {
            if (!_variables.TryAdd(variable.Value, null))
                throw new InvalidOperationException($"The variable with name \"{variable.Value}\" already defined");
        }
        private void AssignVariable(ValuedToken variable, int value)
        {
            if (!IsVariableDefined(variable))
                throw new InvalidVariableException($"The variable with name \"{variable.Value}\" is not defined");
            _variables[variable.Value] = value;
        }
        private int GetVariableValue(ValuedToken variable)
        {
            if (!IsVariableDefined(variable))
                throw new InvalidVariableException($"The variable with name \"{variable.Value}\" is not defined");
            if (!IsVariableAssigned(variable))
                throw new InvalidVariableException($"The variable with name \"{variable.Value}\" is not assigned");
            return (int)_variables[variable.Value];
        }

        public void Interpretate(List<ValuedToken> variables, List<IStatement> statements)
        {
            foreach (ValuedToken variable in variables)
                DefineVariable(variable);
            ExecuteInternalStatements(statements);
        }
        private void ExecuteInternalStatements(List<IStatement> statements)
        {
            foreach (IStatement statement in statements)
                ExecuteStatement(statement);
        }
        private void ExecuteStatement(IStatement statement)
        {
            switch (statement.Type)
            {
                case StatementType.WRITE_STATEMENT:
                    ExecuteWriteStatement((WriteStatement)statement);
                    break;
                case StatementType.READ_STATEMENT:
                    ExecuteReadStatement((ReadStatement)statement);
                    break;
                case StatementType.FOR_STATEMENT:
                    ExecuteForStatement((ForStatement)statement);
                    break;
                case StatementType.ASSIGNMENT_STATEMENT:
                    ExecuteAssignmentStatement((AssignmentStatement)statement);
                    break;
                default:
                    throw new InvalidOperationException("Illegal statement in the program");
            }
        }
        private void ExecuteWriteStatement(WriteStatement statement)
        {
            foreach (var variable in statement.Variables)
            {
                if (IsVariableAssigned(variable))
                    Console.WriteLine($"Value of variable with name \"{variable.Value}\" is {_variables[variable.Value]}");
                else
                    throw new InvalidVariableException($"The variable with name \"{variable.Value}\" is not assigned");
            }
        }
        private void ExecuteReadStatement(ReadStatement statement)
        {
            foreach (var variable in statement.Variables)
            {
                Console.Write($"Input int value to assign variable with name \"{variable.Value}\": ");
                var value = Console.ReadLine();
                AssignVariable(variable, ExpressionHelper.ParseToInt(value));
            }
        }
        private void ExecuteAssignmentStatement(AssignmentStatement statement)
        {
            var value = ExpressionHelper.EvaluateInfixExpressionWithVariables(statement.ValueInfixExpression, GetVariableValue);
            AssignVariable(statement.Variable, value);
        }
        private void ExecuteForStatement(ForStatement statement)
        {
            ExecuteAssignmentStatement(statement.IteratorStatement);
            var iterator = statement.IteratorStatement.Variable;
            int maxIteratorValue = ExpressionHelper.EvaluateInfixExpressionWithVariables(statement.EndIteratorValueExpression, GetVariableValue);
            while (GetVariableValue(iterator) < maxIteratorValue)
            {
                ExecuteInternalStatements(statement.Statements);
                AssignVariable(iterator, GetVariableValue(iterator) + 1);
            }
        }
    }
}
