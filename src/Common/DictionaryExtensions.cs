namespace Common;

public static class DictionaryExtensions
{
    public static void AddToSet<TKey, TValue, TStore>(this Dictionary<TKey, TStore> dictionary, TKey key, TValue value)
        where TKey : notnull
        where TStore: ICollection<TValue>, new()
    {
        if (!dictionary.TryGetValue(key, out var set))
        {
            set = new();
            dictionary[key] = set;
        }
        set.Add(value);
    }
}