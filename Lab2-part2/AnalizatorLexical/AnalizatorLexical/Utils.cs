using AnalizatorLexical.Automata;

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

    public static FiniteAutomata ParseFiniteAutomata(string path)
    {
        var states = new HashSet<string>();
        var alphabet = new HashSet<string>();
        var transitions = new Dictionary<Pair, string>();
        var finalStates = new HashSet<string>();
        
        try
        {
            var lines = File.ReadAllLines(path);
            var data = lines[0].Split(' ');
            foreach (var d in data)
            {
                states.Add(d);
            }

            var initialState = lines[1];

            data = lines[2].Split(' ');
            foreach (var d in data)
            {
                finalStates.Add(d);
            }

            var num = int.Parse(lines[3]);
            var lineIndex = 4;

            for (var i = 0; i < num; i++)
            {
                data = lines[lineIndex].Split(' ');
                string chars;
                if (data.Length > 3)
                {
                    chars = data[2] + " " + data[3];
                }
                else
                {
                    chars = data[2];
                }
                foreach (var chr in chars)
                {
                    alphabet.Add(chr.ToString());
                    var pair = new Pair(data[0], chr.ToString());
                    transitions[pair] = data[1];
                }
                lineIndex++;
            }

            return new FiniteAutomata(states, alphabet, transitions, initialState, finalStates);
        }
        catch (Exception e)
        {
            Console.Write("An error occured while parsing the AF: ");
            Console.Write(e.Message);
            throw;
        }
    }
}