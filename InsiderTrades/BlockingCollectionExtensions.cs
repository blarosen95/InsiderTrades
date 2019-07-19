using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsiderTrades
{
    public static class BlockingCollectionExtensions
    {
        public static void Clear<T>(this BlockingCollection<T> blockingCollection)
        {
            if (blockingCollection == null)
            {
                throw new ArgumentNullException("blockingCollection");
            }

            while (blockingCollection.Count > 0)
            {
                T item;
                blockingCollection.TryTake(out item);
            }
        }
    }
}
