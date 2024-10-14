using Crab.Errors;

namespace Crab.Collections;

/// <summary>
/// Extension methods for mapping and filtering collections.
/// </summary>
public static class MapFilterExtension
{
    /// <summary>
    /// Maps and filters a collection.
    /// </summary>
    /// <typeparam name="T">Input type</typeparam>
    /// <typeparam name="U">Output type</typeparam>
    /// <param name="source"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public static IEnumerable<U> MapFilter<T, U>(this IEnumerable<T> source, Func<T, IOption<U>> map)
    {
        foreach (var item in source)
        {
            var result = map(item);
            if (result.TryUnwrap(out var value))
                yield return value;
        }
    }
}