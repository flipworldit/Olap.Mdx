namespace OLAP.Mdx.LinqExt.CollectionAdapters
{
    public interface IVisitCorrection<T>
    {
        void Add(T element);

        T Pop();

        int Count { get; }
    }
}