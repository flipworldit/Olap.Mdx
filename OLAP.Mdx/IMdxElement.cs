using System.Collections.Generic;
using OLAP.Mdx.MdxElements;

namespace OLAP.Mdx
{
    public interface IMdxElement
    {
        void Draw(MdxDrawContext dc);

        IEnumerable<IMdxElement> GetChildren();
    }
}