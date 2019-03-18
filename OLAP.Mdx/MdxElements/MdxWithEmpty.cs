using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxWithEmpty : IMdxElement
    {
        private List<IMdxElement> _mdxBuilders;

        public MdxWithEmpty(params IMdxElement[] mdxElements)
        {
            _mdxBuilders = new List<IMdxElement>(mdxElements);
        }
        
        public void Draw(MdxDrawContext dc)
        {
            var i = 0;
            foreach (var mdxBuilder in _mdxBuilders)
            {
                mdxBuilder.Draw(dc);
                dc.Append(i < _mdxBuilders.Count - 1 ? "," : "");

                i++;
            }
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return _mdxBuilders;
        }
    }
}