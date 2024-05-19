namespace Models.Extensions;

public static class CollectionExtensions
{
    public static T GetAtOrFirst<T>(this IEnumerable<T> source, int index)
    {
        var array = source?.ToArray();

        if (array == null || array.Length == 0)
        {
            return default;
        }
        
        return index < array.Length
            ? array[index]
            : array[0];
    }
    
    public static double SumWhereNotNull(this IEnumerable<double?>? source)
    {
        if (source == null)
        {
            return default;
        }
        return source.Where(x => x.HasValue).OfType<double>().Sum();
    }
}