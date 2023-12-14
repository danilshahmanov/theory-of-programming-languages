using TheoryOfProgrammingLanguages.Source.Tokens;
using TheoryOfProgrammingLanguages.Source.Statements;
using TheoryOfProgrammingLanguages.Source;

//init alphabet
List<TokenDefinition> tokenDefinitions = new();
tokenDefinitions.Add(new TokenDefinition(@"^\b(var|VAR)\b", TokenType.VAR));
tokenDefinitions.Add(new TokenDefinition(@"^\s+", TokenType.IGNORE));
tokenDefinitions.Add(new TokenDefinition("^;", TokenType.SEMICOLON));
tokenDefinitions.Add(new TokenDefinition("^\\(", TokenType.OPEN_PARENTHESIS));
tokenDefinitions.Add(new TokenDefinition("^\\)", TokenType.CLOSE_PARENTHESIS));
tokenDefinitions.Add(new TokenDefinition("^,", TokenType.COMMA));
tokenDefinitions.Add(new TokenDefinition("^:", TokenType.COLON));
tokenDefinitions.Add(new TokenDefinition(@"^\b(begin|BEGIN)\b", TokenType.BEGIN));
tokenDefinitions.Add(new TokenDefinition(@"^\b(end|END)\b", TokenType.END));
tokenDefinitions.Add(new TokenDefinition(@"^\b(read|READ)\b", TokenType.READ));
tokenDefinitions.Add(new TokenDefinition(@"^\b(write|WRITE)\b", TokenType.WRITE));
tokenDefinitions.Add(new TokenDefinition(@"^\b(for|FOR)\b", TokenType.FOR));
tokenDefinitions.Add(new TokenDefinition(@"^\b(to|TO)\b", TokenType.TO));
tokenDefinitions.Add(new TokenDefinition(@"^\b(do|DO)\b", TokenType.DO));
tokenDefinitions.Add(new TokenDefinition(@"^\b(end_for|END_FOR)\b", TokenType.END_FOR));
tokenDefinitions.Add(new TokenDefinition(@"^\d+", TokenType.NUMBER));
tokenDefinitions.Add(new TokenDefinition(@"^(?!(var|VAR|begin|BEGIN|end|END|integer|INTEGER|read|READ|write|WRITE|for|FOR|to|TO|do|DO))[a-z]+", TokenType.IDENTIFIER));
tokenDefinitions.Add(new TokenDefinition(@"^\b(integer|INTEGER)\b", TokenType.INTEGER));
tokenDefinitions.Add(new TokenDefinition(@"^=", TokenType.ASSIGNMENT_OPERATOR));
tokenDefinitions.Add(new TokenDefinition(@"^\+", TokenType.ADDITION_OPERATOR));
tokenDefinitions.Add(new TokenDefinition(@"^-", TokenType.SUBTRACTION_OPERATOR));
tokenDefinitions.Add(new TokenDefinition(@"^\*", TokenType.MULTIPLICATION_OPERATOR));
tokenDefinitions.Add(new TokenDefinition(@"^/", TokenType.DIVISION_OPERATOR));
tokenDefinitions.Add(new TokenDefinition(@"^!", TokenType.NEGATE_OPERATOR));


string program= @"
    var a,b, n, i, j: integer;
    begin
      read(a);
        write(a);
    
      for i = 3 to a do
        for j = 4 to 9 do
            n=i+j;
            write(n);
        end_for
    end_for
    b=7;
    n = (!5*8*(2+a)+(7-5+b-9*(5*5)+4)); 
    write(n); 
    end
    ";

try
{
    var lexer = new Lexer(tokenDefinitions, program);
    var tokens = lexer.GetTokens();   
    var parser = new Parser(tokens);
    List<ValuedToken> variables;
    List<IStatement> statements;
    parser.ParseProgram(out variables, out statements);  
    var interpreter = new Interpreter();
    interpreter.Interpretate(variables, statements);
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message.ToString());
}