using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxPrirostPercent : IMdxElement
    {
        private IMdxElement _mdxElement;
        private IMdxElement _measure;

        public MdxPrirostPercent(IMdxElement mdxElement, IMdxElement measure)
        {
            _mdxElement = mdxElement;
            _measure = measure;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("Divide (");

            _measure.Draw(dc);
            
            dc.Append(" - Sum (");

            _mdxElement.Draw(dc);

            dc.Append(".PREVMEMBER, ");

            _measure.Draw(dc);

            dc.Append("), ");

            _measure.Draw(dc);
            
            dc.Append(", 0)");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }
    }
}