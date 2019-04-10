using System;
using System.Collections.Generic;
using OLAP.Mdx.LinqExt.CollectionAdapters;

namespace OLAP.Mdx.LinqExt
{
    public static class LinqExtendedDescendans
    {
        private static IEnumerable<T> DescendansQueue<T>(IVisitCorrection<T> q, Func<T, bool> selector, Func<T, IEnumerable<T>> childs)
        {
            while (q.Count != 0)
            {
                var control = q.Pop();
                if (selector(control))
                    yield return control;

                AddChilds(q, childs, control);
            }
        }

        private static void AddChilds<T>(IVisitCorrection<T> q, Func<T, IEnumerable<T>> childs, T control)
        {
            foreach (var child in childs(control))
                q.Add(child);
        }

        public static IEnumerable<T> DescendansAllLayerBFS<T>(T parent, Func<T, bool> selector, Func<T, IEnumerable<T>> childs)
        {
            var q = new CollectionFIFO<T>();

            AddChilds(q, childs, parent);

            return DescendansQueue(q, selector, childs);
        }

        public static IEnumerable<T> DescendansAllLayerDFS<T>(T parent, Func<T, bool> selector, Func<T, IEnumerable<T>> childs)
        {
            //*/
            var q = new CollectionLIFO<T>();

            AddChilds(q, childs, parent);

            return DescendansQueue(q, selector, childs);
        }
    }
}
