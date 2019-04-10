using System.Collections.Generic;

namespace OLAP.Mdx.LinqExt.CollectionAdapters
{
    public class CollectionFIFO<T>
        : Queue<T>, IVisitCorrection<T>
    {
        public void Add(T element)
        {
            Enqueue(element);
        }

        public T Pop()
        {
            return Dequeue();
        }
    }
}