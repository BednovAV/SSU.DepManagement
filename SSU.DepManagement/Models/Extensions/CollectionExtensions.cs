namespace Models.Extensions;

public static class CollectionExtensions
{
    public static T GetAtOrFirst<T>(this IEnumerable<T> source, int index)
    {
        var array = source.ToArray();
        return index < array.Length
            ? array[index]
            : array[0];
    }
}