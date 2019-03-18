using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxDistinctCount : IMdxElement
    {
        private List<IMdxElement> _mdxBuilders;

        public MdxDistinctCount(params IMdxElement[] mdxElements)
        {
            _mdxBuilders = new List<IMdxElement>(mdxElements);
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("DistinctCount (");

            int i = 0;
            foreach (var mdxBuilder in _mdxBuilders)
            {
                mdxBuilder.Draw(dc);
                dc.Append(i < _mdxBuilders.Count - 1 ? "," : "");

                i++;
            }

            dc.Append(")");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return _mdxBuilders;
        }

    }
}