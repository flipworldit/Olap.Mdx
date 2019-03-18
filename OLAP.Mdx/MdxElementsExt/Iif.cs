using OLAP.Mdx.MdxElements;

namespace OLAP.Mdx.MdxElementsExt
{
    public static class Iif
    {
        public static MdxIif SumPrice(string dimension)
        {
            return new MdxIif(
                new MdxMeasureEqualsZero(dimension),
                new MdxNull(),
                new MdxDivide("SumPrice",
                    dimension));
        }
            
    }
}
