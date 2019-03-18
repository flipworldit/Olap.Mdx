using OLAP.Mdx;

namespace Dashboard.OLAP.Mdx.Tests
{
    public class TestMdxBuilderFactory
    {
        public static IMdxBuilder CreateBuilder()
        {
            return new MdxBuilder();
        }
    }
}
