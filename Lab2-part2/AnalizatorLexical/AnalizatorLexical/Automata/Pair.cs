namespace AnalizatorLexical.Automata;

public class Pair
{ 
    public string InitialState { get; }
    public string Value { get; }

    public Pair(string initialState, string value)
    {
        InitialState = initialState;
        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (this == obj) return true;
        if (obj == null || GetType() != obj.GetType()) return false;
        var pair = (Pair)obj;
        return InitialState == pair.InitialState && Value == pair.Value;
    }
    
    public override int GetHashCode()
    { 
        return HashCode.Combine(InitialState, Value);
    }
}