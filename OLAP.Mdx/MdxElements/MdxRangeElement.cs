using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxRangeElement
        : IMdxElement
    {
        private MdxValueElement _first;
        private MdxValueElement _second;

        public MdxRangeElement(MdxValueElement first, MdxValueElement second)
        {
            _first = first;
            _second = second;
        }


        public void Draw(MdxDrawContext dc)
        {
            _first.Draw(dc);
            dc.Append(":");
            _second.Draw(dc);
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }
    }
}