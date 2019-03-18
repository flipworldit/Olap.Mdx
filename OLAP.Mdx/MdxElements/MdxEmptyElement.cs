using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxEmptyElement : IMdxElement
    {
        public MdxEmptyElement()
        {
        }

        public void Draw(MdxDrawContext dc)
        {
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }
    }
}