using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxNot
        : IMdxElement
    {
        public void Draw(MdxDrawContext dc)
        {
            dc.Append("Not");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }
    }

    public class MdxOr
    : IMdxElement
    {
        public void Draw(MdxDrawContext dc)
        {
            dc.Append("Or");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }
    }

}
