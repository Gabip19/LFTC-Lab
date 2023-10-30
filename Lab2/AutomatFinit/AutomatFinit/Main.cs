namespace AutomatFinit;

public static class Main
{
    private static readonly HashSet<string> _states = new();
    private static readonly HashSet<string> _alphabet = new();
    private static readonly Dictionary<Pair, string> _transitions = new();
    private static string _initialState = string.Empty;
    private static readonly HashSet<string> _finalStates = new();
    private static FiniteAutomata _af;

    private static void PrintElementsMenu()
    {
        Console.WriteLine("\n1. States set");
        Console.WriteLine("2. Alphabet");
        Console.WriteLine("3. Transitions");
        Console.WriteLine("4. Final states set");
        Console.WriteLine("0. EXIT\n");
    }

    private static void PrintElementsRun()
    {
        while (true)
        {
            PrintElementsMenu();
            var cmd = Console.ReadLine();
            switch (cmd)
            {
                case "1":
                    _af.PrintStatesSet();
                    break;
                case "2":
                    _af.PrintAlphabet();
                    break;
                case "3":
                    _af.PrintTransitions();
                    break;
                case "4":
                    _af.PrintFinalStates();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Wrong command.\n");
                    break;
            }
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine("\n===============================");
        Console.WriteLine("1. Print automata elements");
        Console.WriteLine("2. Check if sequence is accepted");
        Console.WriteLine("3. Get the longest accepted prefix");
        Console.WriteLine("0. EXIT");
        Console.WriteLine("===============================");
    }

    private static void Run()
    {
        while (true)
        {
            PrintMenu();
            Console.WriteLine("Command: ");
            var cmd = Console.ReadLine();
            switch (cmd)
            {
                case "1":
                    PrintElementsRun();
                    break;
                case "2":
                    var isAfd = _af.CheckAfd();
                    if (isAfd)
                    {
                        Console.Write("Sequence: ");
                        var sequence = Console.ReadLine()!;
                        var isAccepted = _af.CheckSequence(sequence);
                        Console.WriteLine(isAccepted
                            ? "Valid sequence.\n"
                            : "Sequence not accepted.\n");
                    }
                    else
                        Console.WriteLine("Non-determinist finite automata.\n");
                    break;
                case "3":
                    isAfd = _af.CheckAfd();
                    if (isAfd)
                    {
                        Console.Write("Sequence: ");
                        var sequence = Console.ReadLine()!;
                        var prefix = _af.FindLongestPrefix(sequence);
                        Console.WriteLine("Longest accepted prefix: " + prefix);
                    }
                    else
                        Console.WriteLine("Non-determinist finite automata.\n");
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Wrong command.\n");
                    break;
            }
        }
    }

    private static string Start()
    {
        Console.Write("Choose input type:\n\t1. From file.\n\t2. From keyboard.\n\t0. Exit.\nCommand: ");
        while (true)
        {
            var cmd = Console.ReadLine();
            switch (cmd)
            {
                case "1":
                    ReadFromFile();
                    return cmd;
                case "2":
                    ReadFromKeyboard();
                    return cmd;
                case "0":
                    return cmd;
                default:
                    Console.WriteLine("Wrong command.");
                    break;
            }
        }
    }

    private static void ReadFromFile()
    {
        Console.Write("File path: ");
        var filePath = Console.ReadLine()!;
        try
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var data = line.Split(' ');
                foreach (var d in data)
                {
                    _states.Add(d);
                }

                _initialState = lines[1];

                data = lines[2].Split(' ');
                foreach (var d in data)
                {
                    _finalStates.Add(d);
                }

                var num = int.Parse(lines[3]);
                var lineIndex = 4;

                for (var i = 0; i < num; i++)
                {
                    data = lines[lineIndex].Split(' ');
                    foreach (var chr in data[2])
                    {
                        _alphabet.Add(chr.ToString());
                        var pair = new Pair(data[0], chr.ToString());
                        _transitions[pair] = data[1];
                    }
                    lineIndex++;
                }

                _af = new FiniteAutomata(_states, _alphabet, _transitions, _initialState, _finalStates);
            }
        }
        catch (Exception e)
        {
            Console.Write("An error occured: ");
            Console.Write(e.Message);
        }
    }

    private static void ReadFromKeyboard()
    {
        try
        {
            Console.Write("States set: ");
            var inputStates = Console.ReadLine()!.Split(' ');
            foreach (var s in inputStates)
            {
                _states.Add(s);
            }

            while (true)
            {
                Console.Write("Initial state: ");
                _initialState = Console.ReadLine()!;
                if (!_states.Contains(_initialState))
                {
                    Console.WriteLine("Initial state must be an existing state.\n");
                }
                else
                {
                    break;
                }
            }

            while (true)
            {
                var ok = true;
                Console.Write("Final states: ");
                var finalStates = Console.ReadLine()!.Split(' ');
                if (finalStates.Any(s => !_states.Contains(s)))
                {
                    Console.WriteLine("Final states must be from the existing states.\n");
                    ok = false;
                }

                if (!ok) continue;
                foreach (var s in finalStates)
                {
                    _finalStates.Add(s);
                }
                break;
            }

            Console.Write("Number of transitions: ");
            var num = int.Parse(Console.ReadLine()!);
            
            Console.WriteLine("Transitions (initialState, endState, value):");
            for (var i = 0; i < num; i++)
            {
                var data = Console.ReadLine()!.Split(' ');
                if (!_states.Contains(data[0]) || !_states.Contains(data[1]))
                {
                    throw new Exception("States must be from the existing states.");
                }

                foreach (var chr in data[2])
                {
                    _alphabet.Add(chr.ToString());
                    _transitions[new Pair(data[0], chr.ToString())] = data[1];
                }
            }

            _af = new FiniteAutomata(_states, _alphabet, _transitions, _initialState, _finalStates);
        }
        catch (Exception e)
        {
            Console.Write("\nAn error occured: ");
            Console.Write(e.Message);
        }
    }

    public static void StartApp()
    {
        try
        {
            var cmd = Start();
            if (cmd is "1" or "2")
            {
                Run();
            }
        }
        catch (Exception e)
        {
            Console.Write("\nAn error occured: ");
            Console.Write(e.Message);
        }
    }
}