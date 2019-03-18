using System.Collections.Generic;
using System.Linq;
using LinqExtIn;

namespace OLAP.Mdx.MdxElements
{
    public class MdxNonEmpty : IMdxElement, IMdxCollectionElements
    {
        private List<IMdxElement> _mdxBuilders;

        public MdxNonEmpty(params IMdxElement[] mdxElements)
        {
            _mdxBuilders = new List<IMdxElement>(mdxElements);
        }

        public MdxNonEmpty(IEnumerable<IMdxElement> mdxElements)
        {
            _mdxBuilders = new List<IMdxElement>(mdxElements);
        }
        
        public void Draw(MdxDrawContext dc)
        {
            dc.Append("NON EMPTY ");
            dc.OpenBracket("(");

            int i = 0;
            foreach (var mdxBuilder in _mdxBuilders)
            {
                mdxBuilder.Draw(dc);
                dc.Append(i < _mdxBuilders.Count - 1 ? "," : "");

                i++;
            }

            dc.CloseBracket(")");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return _mdxBuilders;
        }

        public void RemoveChild(IMdxElement child)
        {
            _mdxBuilders.Remove(child);
        }

        public void ReplaceChild(string name, IMdxElement newChildren)
        {
            throw new System.NotImplementedException();
        }

        public IMdxCollectionElements AddRange(IEnumerable<IMdxElement> measures)
        {
            throw new System.NotImplementedException();
        }

        public bool IsEmpty()
        {
            return _mdxBuilders.Count == 0;
        }
    }

    public class MdxNonempty : IMdxElement
    {
        private List<IMdxElement> _mdxBuilders;

        public MdxNonempty(params IMdxElement[] mdxElements)
        {
            _mdxBuilders = new List<IMdxElement>(mdxElements);
        }

        public MdxNonempty(IEnumerable<IMdxElement> mdxElements, IEnumerable<IMdxElement> mdxElements2)
        {
            _mdxBuilders = new List<IMdxElement>(mdxElements);

            _mdxBuilders.AddRange(mdxElements2);
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("NONEMPTY ");
            dc.OpenBracket("(");

            int i = 0;
            foreach (var mdxBuilder in _mdxBuilders)
            {
                mdxBuilder.Draw(dc);
                dc.Append(i < _mdxBuilders.Count - 1 ? "," : "");

                i++;
            }

            dc.CloseBracket(")");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return _mdxBuilders;
        }
    }

}