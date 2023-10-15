using System.Text.RegularExpressions;

namespace AnalizatorLexical;

public class Lexer
{
    private readonly IDictionary<string, int> _atomTable;
    private readonly char[] _separators = { ';', '(', ')', ',', '{', '}' };
    
    public Lexer(IDictionary<string, int> atomTable)
    {
        _atomTable = atomTable;
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
            return;
        }
        
        if (IsConstant(token))
        {
            return;
        }
        
        if (IsIdentifier(token))
        {
            if (token.Length >= 250)
            {
                throw new InvalidTokenException("Identifier can not be longer than 250 characters.");
            }
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