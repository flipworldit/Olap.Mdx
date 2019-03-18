using OLAP.Mdx.MdxElements;

namespace OLAP.Mdx.MesuareCreators
{
    public class MeasureCreator : IMeasureCreator
    {
        public IMdxElement Create(string measure)
        {
            return new MdxMeasureElement(measure);
        }
    }
}