using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxPrirost : IMdxElement
    {
        private IMdxElement _mdxElement;
        private IMdxElement _measure;

        public MdxPrirost(IMdxElement mdxElement, IMdxElement measure)
        {
            _mdxElement = mdxElement;
            _measure = measure;
        }

        public void Draw(MdxDrawContext dc)
        {
            _measure.Draw(dc);
            
            dc.Append(" - Sum (");

            _mdxElement.Draw(dc);

            dc.Append(".PREVMEMBER, ");

            _measure.Draw(dc);

            dc.Append(")");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }
    }
}