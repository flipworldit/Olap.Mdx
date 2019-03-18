using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxSum : IMdxElement
    {
        private IMdxElement _mdxElement;
        private IMdxElement _measure;

        public MdxSum(IMdxElement mdxElement, MdxMeasureElement measure)
            : this(mdxElement, (IMdxElement)measure)
        {
            
        }

        public MdxSum(IMdxElement mdxElement, IMdxElement measure)
        {
            _mdxElement = mdxElement;
            _measure = measure;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("Sum (");

            _mdxElement.Draw(dc);

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