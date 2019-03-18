using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxPeriodsToDate : IMdxElement
    {
        private IMdxElement _mdxElement;
        private IMdxElement _measure;

        public MdxPeriodsToDate(IMdxElement mdxElement, IMdxElement measure)
        {
            _mdxElement = mdxElement;
            _measure = measure;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("PeriodsToDate (");

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