using System.Collections.Generic;
using System.Linq;

namespace OLAP.Mdx
{
    public static class IMdxElementExt
    {
        public static bool Remove(this IMdxElement mdxElement, params IMdxElement[] deleteNodes)
        {
            return Remove(mdxElement, new HashSet<IMdxElement>(deleteNodes));
        }

        public static bool Remove(this IMdxElement mdxElement, IEnumerable<IMdxElement> deleteNodes)
        {
            return Remove(mdxElement, new HashSet<IMdxElement>(deleteNodes));
        }

        private static bool Remove(IMdxElement mdxElement, HashSet<IMdxElement> deleteNodes)
        {
            if (deleteNodes.Contains(mdxElement))
            {
                return true;
            }

            var result = false;

            var children = mdxElement
                .GetChildren()
                .ToArray();

            foreach (var child in children)
            {
                var removeFromChild = Remove(child, deleteNodes);

                if (removeFromChild)
                {
                    var mdxCollection = mdxElement as IMdxCollectionElements;

                    if (mdxCollection != null)
                    {
                        mdxCollection.RemoveChild(child);

                        removeFromChild = mdxCollection.IsEmpty();
                    }
                }

                result = removeFromChild || result;
            }

            return result;
        }
    }
}