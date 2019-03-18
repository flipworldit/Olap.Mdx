using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxSubset : IMdxElement
    {
        private IMdxElement _mdxElement;
        private int _start;
        private int _count;

        public MdxSubset(IMdxElement mdxElement, int start, int count)
        {
            _count = count;
            _start = start;
            _mdxElement = mdxElement;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("SubSet");
            dc.OpenBracket("(");

            _mdxElement.Draw(dc);
            dc.Append(", ");
            dc.Append(_start.ToString());

            dc.Append(", ");
            dc.Append(_count.ToString());

            dc.CloseBracket(")");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            yield return _mdxElement;
        }
    }
}