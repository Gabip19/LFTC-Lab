using AnalizatorLexical;

const string PATH_TO_ATOM_TABLE = @"..\..\..\..\..\atom_table.txt";
const string PATH_TO_PROGRAM = @"..\..\..\..\..\Probleme\Corecte\";

Console.Write("File name: ");
var fileName = Console.ReadLine();

var atomTable = Utils.ParseAtomTable(PATH_TO_ATOM_TABLE);
var lexer = new Lexer(atomTable);

try
{
    var tokens = lexer.GetTokens(PATH_TO_PROGRAM + fileName);
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