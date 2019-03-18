using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxGenerateElement : IMdxElement
    {
        private IMdxElement _mdxRangeElement;
        private IMdxElement _measure;

        public MdxGenerateElement(IMdxElement mdxRangeElement, IMdxElement measure)
        {
            _mdxRangeElement = mdxRangeElement;
            _measure = measure;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("Generate (");

            _mdxRangeElement.Draw(dc);

            dc.Append(", ");

            _measure.Draw(dc);

            dc.Append(")");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }
    }
}