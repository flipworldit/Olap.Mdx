using System.Collections.Generic;
using System.Linq;

namespace OLAP.Mdx.MdxElements
{
    public class MdxMultiplyElements : IMdxElement
    {
        private IMdxElement[] _elements;

        public MdxMultiplyElements(IMdxElement[] elements)
        {
            _elements = elements;
        }

        public void Draw(MdxDrawContext dc)
        {
            for (var i = 0; i < _elements.Count(); i++)
            {
                _elements[i].Draw(dc);

                if (i != _elements.Count() - 1)
                {
                    dc.Append(" * ");
                }
            }

        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }

    }
}