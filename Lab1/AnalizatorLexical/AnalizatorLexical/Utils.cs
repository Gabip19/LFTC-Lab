using System.Text;

namespace AnalizatorLexical;

public static class Utils
{
    public static IDictionary<string, int> ParseAtomTable(string path)
    {
        var lines = File.ReadLines(path);
        return lines
            .Skip(3)
            .Select(line => line.Split(" : "))
            .ToDictionary(words => words[0], words => int.Parse(words[1]));
    }

    public static IEnumerable<string> SplitBySeparators(this string str, char[] separators)
    {
        var tokens = new List<string>();
        var currentToken = new StringBuilder();
        var insideStringConst = false;
        
        foreach (var chr in str)
        {
            if (chr == '\"')
            {
                currentToken.Append(chr);
                insideStringConst = !insideStringConst;
            }
            else if (insideStringConst)
            {
                currentToken.Append(chr);
            }
            else if (char.IsWhiteSpace(chr))
            {
                if (currentToken.Length != 0)
                {
                    tokens.Add(currentToken.ToString());
                    currentToken = new StringBuilder();
                }
            }
            else if (separators.Contains(chr))
            {
                if (currentToken.Length != 0)
                {
                    tokens.Add(currentToken.ToString());
                    currentToken = new StringBuilder();
                }

                tokens.Add(chr.ToString());
            }
            else
            {
                currentToken.Append(chr);
            }
        }

        if (currentToken.Length != 0)
        {
            tokens.Add(currentToken.ToString());
        }
        
        return tokens;
    }
}