using OLAP.Mdx.MdxElements;

namespace OLAP.Mdx.MesuareCreators
{
    public class AggregateAvgCreator : IMeasureCreator
    {
        private readonly MdxRangeElement _range;

        public AggregateAvgCreator(MdxRangeElement range)
        {
            _range = range;
        }

        public IMdxElement Create(string measure)
        {
            return new MdxAvg(_range,measure);
        }
    }
}