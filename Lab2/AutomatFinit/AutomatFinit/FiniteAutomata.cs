namespace AutomatFinit;

public class FiniteAutomata
{
    private readonly HashSet<string> _states;
    private readonly HashSet<string> _alphabet;
    private readonly Dictionary<Pair, string> _transitions;
    private readonly string _initialState;
    private readonly HashSet<string> _finalStates;
    private readonly HashSet<string> _digits = new();

    public FiniteAutomata(
        HashSet<string> states, 
        HashSet<string> alphabet, 
        Dictionary<Pair, string> transitions, 
        string initialState, 
        HashSet<string> finalStates
    )
    {
        _states = states;
        _alphabet = alphabet;
        _transitions = transitions;
        _initialState = initialState;
        _finalStates = finalStates;

        _digits.UnionWith(new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" });
    }

    public bool CheckAfd()
    {
        List<Pair> states = new();
        foreach (var t in _transitions)
        {
            if (states.Contains(t.Key))
                return false;
            states.Add(t.Key);
        }
        return true;
    }

    public bool CheckSequence(string sequence)
    {
        var currentState = _initialState;

        foreach (var currentPair in sequence.Select(chr => new Pair(currentState, chr.ToString())))
        {
            if (_transitions.TryGetValue(currentPair, out var next))
            {
                currentState = next;
            }
            else
            {
                return false;
            }
        }

        return _finalStates.Contains(currentState);
    }

    public string FindLongestPrefix(string sequence)
    {
        var initialState = _initialState;
        var prefix = "";

        for (var i = 0; i < sequence.Length; i++)
        {
            var character = sequence.Substring(i, 1);
            var value = _digits.Contains(character) ? "cifra" : character;
            var currentPair = new Pair(initialState, value);

            if (_transitions.TryGetValue(currentPair, out var next))
            {
                initialState = next;
                prefix += character;
            }
            else
            {
                break;
            }
        }
        return prefix;
    }

    public void PrintStatesSet()
    {
        Console.Write("States set: { ");
        foreach (var element in _states)
        {
            Console.Write(element + " ");
        }
        Console.WriteLine("}.");
    }

    public void PrintAlphabet()
    {
        Console.Write("Input alphabet: { ");
        foreach (var element in _alphabet)
        {
            Console.Write(element + " ");
        }
        Console.WriteLine("}.");
    }

    public void PrintTransitions()
    {
        Console.WriteLine("Transitions:");
        foreach (var t in _transitions)
        {
            Console.WriteLine(t.Key.InitialState + " -- " + t.Key.Value + " --> " + t.Value);
        }
    }

    public void PrintFinalStates()
    {
        Console.Write("Final states: { ");
        foreach (var element in _finalStates)
        {
            Console.Write(element + " ");
        }
        Console.WriteLine("}.");
    }
}