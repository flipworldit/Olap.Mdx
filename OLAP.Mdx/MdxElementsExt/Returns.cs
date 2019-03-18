using OLAP.Mdx.MdxElements;

namespace OLAP.Mdx.MdxElementsExt
{
    public static class Returns
    {
        public static MdxSubtraction SumPriceDiv1000()
        {
            return new MdxSubtraction(
                new MdxEmptyElement(),
                Mesuares.Return.SumPriceDiv1000
                );

        }

        public static MdxSubtraction SumPrice()
        {
            return new MdxSubtraction(
                new MdxEmptyElement(),
                Mesuares.Return.SumPrice
                );

        }

        public static MdxSubtraction SumPriceAll()
        {
            return new MdxSubtraction(
                new MdxEmptyElement(),
                Mesuares.Return.SumPriceAll
                );

        }


        public static MdxMember SumPriceByMonth(MdxRangeElement monthOffset)
        {
            return new MdxMember("sum",
                    new MdxSum(monthOffset,
                        SumPrice()
                        ));
        }

        public static MdxMember SumPriceAllByMonth(MdxRangeElement monthOffset)
        {
            return new MdxMember("sum",
                    new MdxSum(monthOffset,
                        SumPriceAll()
                        ));
        }
    }
}
