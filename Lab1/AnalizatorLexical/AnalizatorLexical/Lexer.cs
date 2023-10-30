using System.Text.RegularExpressions;

namespace AnalizatorLexical;

public class Lexer
{
    private readonly IDictionary<string, int> _atomTable;
    private readonly char[] _separators = { ';', '(', ')', ',', '{', '}' };
    public readonly HashTable<string> ConstantsTable;
    public readonly HashTable<string> IdsTable;
    public List<KeyValuePair<int, int?>> Fip { get; }
    
    public Lexer(IDictionary<string, int> atomTable)
    {
        _atomTable = atomTable;
        ConstantsTable = new StringHashTable();
        IdsTable = new StringHashTable();
        Fip = new List<KeyValuePair<int, int?>>();
    }
    
    public IEnumerable<string> GetTokens(string pathToProgram)
    {
        var tokens = new List<string>();
        var lineNumber = 0;
        
        var programLines = File.ReadLines(pathToProgram);
        foreach (var line in programLines)
        {
            lineNumber++;
            var words = line.SplitBySeparators(_separators);
            foreach (var word in words)
            {
                try
                {
                    ProcessToken(word);
                }
                catch (InvalidTokenException e)
                {
                    throw new InvalidTokenException(lineNumber, e.Message);
                }
                
                tokens.Add(word);
            }
        }

        return tokens;
    }

    private void ProcessToken(string token)
    {
        if (IsKeyword(token))
        {
            Fip.Add(new KeyValuePair<int, int?>(_atomTable[token], null));
            return;
        }
        
        if (IsConstant(token))
        {
            var constCode = ConstantsTable.Contains(token);
            if (constCode == -1)
            {
                constCode = ConstantsTable.Add(token);
            }
            Fip.Add(new KeyValuePair<int, int?>(1, constCode));
            
            return;
        }
        
        if (IsIdentifier(token))
        {
            if (token.Length >= 250)
            {
                throw new InvalidTokenException("Identifier can not be longer than 250 characters.");
            }

            var idCode = IdsTable.Contains(token);
            if (idCode == -1)
            {
                idCode = IdsTable.Add(token);
            }
            Fip.Add(new KeyValuePair<int, int?>(0, idCode));
            
            return;
        }

        throw new InvalidTokenException("Invalid token.");
    }

    private bool IsKeyword(string token)
    {
        return _atomTable.ContainsKey(token);
    }

    private static bool IsConstant(string token)
    {
        var constantRegex = new Regex("^[+-]?[0-9]+$|^[+-]?[0-9]+(\\.[0-9]*)?$|^\"[^\"]*\"$");
        return constantRegex.IsMatch(token);
    }
    
    private static bool IsIdentifier(string token)
    {
        var identifierRegex = new Regex("^[_a-zA-Z]([_a-zA-Z0-9])*$");
        return identifierRegex.IsMatch(token);
    }
}