using OLAP.Mdx.MdxElements;

namespace OLAP.Mdx.MesuareCreators
{
    public class AggregateSumCreator : IMeasureCreator
    {
        private readonly MdxRangeElement _range;

        public AggregateSumCreator(MdxRangeElement range)
        {
            _range = range;
        }

        public IMdxElement Create(string measure)
        {
            return new MdxSum(_range, measure);
        }
    }
}