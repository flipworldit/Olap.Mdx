using OLAP.Mdx.MdxElements;

namespace OLAP.Mdx.MdxElementsExt
{
    public static class Divider
    {
        public static MdxDivideNumber SumPriceDiv1000
        {
            get
            {
                return DivideBy1000("SumPrice");
            }

        }

        public static MdxDivideNumber WeightDiv1000
        {
            get
            {
                return DivideBy1000("Weight");
            }

        }

        public static MdxDivideNumber GrossProfitDiv1000
        {
            get
            {
                return DivideBy1000("grossProfit");
            }
        }

        public static MdxDivideNumber DivideBy1000(string measure)
        {
            return new MdxDivideNumber(
                new MdxMeasureElement(measure),
                1000);
        }

    }
}
