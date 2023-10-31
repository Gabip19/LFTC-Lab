namespace AutomatFinit;

public class FiniteAutomata
{
    private readonly HashSet<string> _states;
    private readonly HashSet<string> _alphabet;
    private readonly Dictionary<Pair, string> _transitions;
    private readonly string _initialState;
    private readonly HashSet<string> _finalStates;

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
        var currentState = _initialState;
        var currentPrefix = string.Empty;
        string? longestPrefix = null;

        foreach (var chr in sequence)
        {
            var currentPair = new Pair(currentState, chr.ToString());
            if (_finalStates.Contains(currentState))
            {
                longestPrefix = currentPrefix;
            }
            if (_transitions.TryGetValue(currentPair, out var next))
            {
                currentState = next;
                currentPrefix += chr;
            }
            else
            {
                break;
            }
        }

        if (_finalStates.Contains(currentState))
        {
            longestPrefix = currentPrefix;
        }
        
        if (longestPrefix is null)
        {
            return string.Empty;
        }
        return longestPrefix == string.Empty ? "eps" : longestPrefix;
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