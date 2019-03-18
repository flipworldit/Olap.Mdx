using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxNull
        : IMdxElement
    {
        public void Draw(MdxDrawContext dc)
        {
            dc.Append("Null");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }
    }
}