using OLAP.Mdx.MdxElements;

namespace OLAP.Mdx.MesuareCreators
{
    public class GrowthMeasureCreator : IMeasureCreator
    {
        private readonly string _periodTypeMesuareName;

        public GrowthMeasureCreator(string periodTypeMesuareName)
        {
            _periodTypeMesuareName = periodTypeMesuareName;
        }

        public IMdxElement Create(string measure)
        {
            return new MdxSubtraction(
                new MdxSum(new MdxHierarchy(_periodTypeMesuareName).CurrentMember(), measure),
                new MdxSum(new MdxHierarchy(_periodTypeMesuareName).PrevMember(), measure));
        }
    }
}