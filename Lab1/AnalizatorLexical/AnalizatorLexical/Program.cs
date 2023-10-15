using AnalizatorLexical;

const string PATH_TO_ATOM_TABLE = @"..\..\..\..\..\atom_table.txt";
const string PATH_TO_PROGRAM = @"..\..\..\..\..\Probleme\Corecte\pb1.cpp";

var atomTable = Utils.ParseAtomTable(PATH_TO_ATOM_TABLE);
var lexer = new Lexer(atomTable);

try
{
    var tokens = lexer.GetTokens(PATH_TO_PROGRAM);
    foreach (var token in tokens)
    {
        Console.WriteLine(token);
    }
}
catch (Exception e)
{
    Console.Error.WriteLine(e.Message);
}