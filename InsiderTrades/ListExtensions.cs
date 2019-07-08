using System.Collections.Generic;
using System.Linq;

namespace InsiderTrades
{
    /// <summary>
    /// Helper methods for the lists.
    /// Courtesy of Dmitry Pavlov
    /// https://stackoverflow.com/a/24087164
    /// </summary>
    public static class ListExtensions
    {
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize) =>
            source
                .Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
    }
}
