using System.Collections.Generic;

namespace LinqExtIn.CollectionAdapters
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