using System.Collections.Generic;

namespace OLAP.Mdx
{
    public interface IMdxCollectionElements
    {
        void RemoveChild(IMdxElement child);
        
        void ReplaceChild(string name, IMdxElement newChildren);
        
        IMdxCollectionElements AddRange(IEnumerable<IMdxElement> measures);

        bool IsEmpty();
    }
}