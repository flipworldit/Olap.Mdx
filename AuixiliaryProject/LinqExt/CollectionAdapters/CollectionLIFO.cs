using System.Collections.Generic;

namespace LinqExtIn.CollectionAdapters
{
    public class CollectionLIFO<T>
        : Stack<T>, IVisitCorrection<T>
    {
        public void Add(T element)
        {
            Push(element);
        }
    }
}