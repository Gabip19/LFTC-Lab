namespace AnalizatorLexical;

public class StringHashTable : HashTable<string>
{
    protected override int GetHash(string item)
    {
        base.GetHash(item);

        return item[0] % _slots.Length;
    }
}