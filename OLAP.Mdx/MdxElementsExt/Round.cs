using OLAP.Mdx.MdxElements;

namespace OLAP.Mdx.MdxElementsExt
{
    public static class Round
    {
        public static MdxRound By2(IMdxElement numeratorMeasure)
        {
            return new MdxRound(
                numeratorMeasure,
                2);
        }

        public static MdxRound By2NextElement(string elementName)
        {
            return By2(new MdxMeasureElement(elementName));
        }
    }
}
