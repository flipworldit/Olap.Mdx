using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxExpression: IMdxElement
    {
        private List<IMdxElement> _mdxBuilders;

        public MdxExpression(params IMdxElement[] mdxElements)
        {
            _mdxBuilders = new List<IMdxElement>(mdxElements);
        }


        public void Draw(MdxDrawContext dc)
        {
            if (_mdxBuilders != null)
            {
                foreach (var item in _mdxBuilders)
                {
                    item.Draw(dc);
                    dc.Append(" ");
                }
            }
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return _mdxBuilders;
        }


    }
}
