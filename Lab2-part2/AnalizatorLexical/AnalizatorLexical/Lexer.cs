using AnalizatorLexical.Automata;

namespace AnalizatorLexical;

public class Lexer
{
    private readonly IDictionary<string, int> _atomTable;
    private readonly char[] _separators = { ';', '(', ')', ',', '{', '}', ':' };

    private readonly FiniteAutomata _keywordAf;
    private readonly FiniteAutomata _operatorAf;
    private readonly FiniteAutomata _identifierAf;
    private readonly FiniteAutomata _intConstAf;
    private readonly FiniteAutomata _floatConstAf;
    private readonly FiniteAutomata _stringConstAf;
    
    public readonly HashTable<string> ConstantsTable;
    public readonly HashTable<string> IdsTable;
    public List<KeyValuePair<int, int?>> Fip { get; }
    
    public Lexer(
        IDictionary<string, int> atomTable, 
        FiniteAutomata keywordAf, 
        FiniteAutomata operatorAf, 
        FiniteAutomata identifierAf, 
        FiniteAutomata intConstAf, 
        FiniteAutomata floatConstAf, 
        FiniteAutomata stringConstAf)
    {
        _atomTable = atomTable;
        _keywordAf = keywordAf;
        _operatorAf = operatorAf;
        _identifierAf = identifierAf;
        _intConstAf = intConstAf;
        _floatConstAf = floatConstAf;
        _stringConstAf = stringConstAf;
        
        ConstantsTable = new StringHashTable();
        IdsTable = new StringHashTable();
        Fip = new List<KeyValuePair<int, int?>>();

        _tokens = new List<string>();
    }

    private List<string> _tokens;
    public IEnumerable<string> GetTokens(string pathToProgram)
    {
        _tokens = new List<string>();
        var lineNumber = 0;
        
        var programLines = File.ReadLines(pathToProgram);
        foreach (var line in programLines)
        {
            lineNumber++;
            try
            {
                ProcessLine(line);
            }
            catch (InvalidTokenException e)
            {
                throw new InvalidTokenException(lineNumber, e.Message);
            }
        }

        return _tokens;
    }

    private void ProcessLine(string line)
    {

        while (line.Length > 0)
        {
            string? elem = null;
            if (char.IsWhiteSpace(line[0]))
            {
                line = line[1..];
                continue;
            }
            
            if (_separators.Contains(line[0]))
            {
                elem = line[0].ToString();
                Fip.Add(new KeyValuePair<int, int?>(_atomTable[elem], null));
            }
            else if (elem is null && (elem = ExtractKeyword(line)) is not null)
            {
                Fip.Add(new KeyValuePair<int, int?>(_atomTable[elem], null));
            }
            else if (elem is null && (elem = ExtractOperator(line)) is not null)
            {
                Fip.Add(new KeyValuePair<int, int?>(_atomTable[elem], null));
            }
            else if (elem is null && (elem = ExtractConstant(line)) is not null)
            {
                var constCode = ConstantsTable.Contains(elem);
                if (constCode == -1)
                {
                    constCode = ConstantsTable.Add(elem);
                }
                Fip.Add(new KeyValuePair<int, int?>(1, constCode));
            }
            else if (elem is null && (elem = ExtractIdentifier(line)) is not null)
            {
                var idCode = IdsTable.Contains(elem);
                if (idCode == -1)
                {
                    idCode = IdsTable.Add(elem);
                }
                Fip.Add(new KeyValuePair<int, int?>(0, idCode));
            }
            else
            {
                throw new InvalidTokenException("Invalid token.");
            }

            _tokens.Add(elem);
            line = line[elem.Length..];
        }
    }

    private string? ExtractOperator(string line)
    {
        var elem = _operatorAf.FindLongestPrefix(line);

        return elem;
    }

    private string? ExtractKeyword(string line)
    {
        var elem = _keywordAf.FindLongestPrefix(line);

        if (elem == null || !_atomTable.ContainsKey(elem))
        {
            return null;
        }

        return elem;
    }

    private string? ExtractConstant(string line)
    {
        var elem = _floatConstAf.FindLongestPrefix(line) ??
                   _intConstAf.FindLongestPrefix(line) ??
                   _stringConstAf.FindLongestPrefix(line);

        return elem;
    }

    private string? ExtractIdentifier(string line)
    {
        var elem = _identifierAf.FindLongestPrefix(line);

        if (elem?.Length > 250)
        {
            throw new InvalidTokenException("Identifier can not be longer than 250 characters");
        }

        return elem;
    }
}