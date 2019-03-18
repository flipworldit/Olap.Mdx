using System.Collections.Generic;
using System.Linq;
using LinqExtIn;

namespace OLAP.Mdx.MdxElements
{
    public class TypedMdxElement
        : IMdxElement, IMdxCollectionElements
    {
        public List<IMdxElement> _measures = new List<IMdxElement>();

        public TypedMdxElement()
        {
        }

        public TypedMdxElement(params IMdxElement[] measures)
        {
            AddRange(measures);
        }

        public TypedMdxElement(params MdxMeasureElement[] measures)
        {
            AddRange(measures);
        }

        public TypedMdxElement(IEnumerable<IMdxElement> measures)
        {
            AddRange(measures);
        }

        public IMdxCollectionElements AddRange(IEnumerable<IMdxElement> measures)
        {
            foreach (var measure in measures)
            {
                if (measure.GetType() == typeof(MdxHierarchy))
                {
                    ((MdxHierarchy)measure).parentMdx = this;
                }
            }

            _measures.AddRange(measures);

            return this;
        }

        public bool IsEmpty()
        {
            return _measures.Count == 0;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.OpenBracket();

            int i = 0;
            foreach (var measure in _measures)
            {
                dc.BeginLine();

                measure.Draw(dc);
                dc.Append(i < _measures.Count - 1 ? "," : "");

                dc.EndOfLine();
                i++;
            }

            dc.CloseBracket();
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return _measures;
        }

        public void RemoveChild(IMdxElement child)
        {
            _measures.Remove(child);
        }

        public void ReplaceChild(string name, IMdxElement newChildren)
        {
            for (var i = 0; i < _measures.Count; i++)
            {
                var measure = _measures[i];

                if (measure.GetType() != typeof(MdxHierarchy))
                {
                    continue;
                }

                if (((MdxHierarchy)measure).Name == name)
                {
                    _measures[i] = newChildren;
                }
            }
        }
    }
}