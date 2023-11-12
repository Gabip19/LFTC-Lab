using AnalizatorLexical;

#region PATH CONSTS

const string PATH_TO_ATOM_TABLE = @"..\..\..\..\..\atom_table.txt";

const string PATH_TO_KEYWORD_AF = @"..\..\..\..\..\afs\keyword.txt";
const string PATH_TO_OPERATOR_AF = @"..\..\..\..\..\afs\operator.txt";
const string PATH_TO_ID_AF = @"..\..\..\..\..\afs\identifier.txt";
const string PATH_TO_INT_AF = @"..\..\..\..\..\afs\integer.txt";
const string PATH_TO_FLOAT_AF = @"..\..\..\..\..\afs\float.txt";
const string PATH_TO_STRING_AF = @"..\..\..\..\..\afs\string.txt";

const string PATH_TO_PROGRAM = @"..\..\..\..\..\Probleme\Corecte\";

#endregion

var atomTable = Utils.ParseAtomTable(PATH_TO_ATOM_TABLE);

var keywordAf = Utils.ParseFiniteAutomata(PATH_TO_KEYWORD_AF);
var operatorAf = Utils.ParseFiniteAutomata(PATH_TO_OPERATOR_AF);
var identifierAf = Utils.ParseFiniteAutomata(PATH_TO_ID_AF);
var integerAf = Utils.ParseFiniteAutomata(PATH_TO_INT_AF);
var floatAf = Utils.ParseFiniteAutomata(PATH_TO_FLOAT_AF);
var stringAf = Utils.ParseFiniteAutomata(PATH_TO_STRING_AF);

Console.Write("File name: ");
var fileName = Console.ReadLine();

var lexer = new Lexer(
    atomTable: atomTable,
    keywordAf: keywordAf,
    operatorAf: operatorAf,
    identifierAf: identifierAf,
    intConstAf: integerAf,
    floatConstAf: floatAf,
    stringConstAf: stringAf
);

try
{
    var tokens = lexer.GetTokens(PATH_TO_PROGRAM + fileName);
    foreach (var token in tokens)
    {
        Console.WriteLine(token);
    }
    Console.WriteLine("\n===== FIP ======");
    foreach (var pair in lexer.Fip)
    {
        Console.Write(pair.Key + "\t");
        Console.Write(pair.Value == null ? "." : pair.Value);
        Console.Write("\n");
    }

    Console.WriteLine("\n===== TS-CONST =====");
    foreach (var elem in lexer.ConstantsTable.Items)
    {
        Console.WriteLine(elem + "\t" + lexer.ConstantsTable.Contains(elem));
    }

    Console.WriteLine("\n===== TS-IDS =====");
    foreach (var elem in lexer.IdsTable.Items)
    {
        Console.WriteLine(elem + "\t" + lexer.IdsTable.Contains(elem));
    }
}
catch (Exception e)
{
    Console.Error.WriteLine(e.Message);
}