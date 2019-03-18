using System.Collections.Generic;
using System.Linq;
using LinqExtIn;

namespace OLAP.Mdx.MdxElements
{
    public class UnionMdxElement
        : IMdxElement, IMdxCollectionElements
    {
        public List<IMdxElement> Measures = new List<IMdxElement>();

        public UnionMdxElement()
        {
        }

        public UnionMdxElement(params IMdxElement[] measures)
        {
            AddRange(measures);
        }

        public UnionMdxElement(params MdxMeasureElement[] measures)
        {
            AddRange(measures);
        }

        public UnionMdxElement(IEnumerable<IMdxElement> measures)
        {
            AddRange(measures);
        }

        public IMdxCollectionElements AddRange(IEnumerable<IMdxElement> measures)
        {
            var mdxElements = measures as IMdxElement[] ?? measures.ToArray();

            foreach (var measure in mdxElements)
            {
                if (measure.GetType() == typeof (MdxHierarchy))
                {
                    ((MdxHierarchy) measure).parentMdx = this;
                }
            }

            Measures.AddRange(mdxElements);

            return this;
        }

        public bool IsEmpty()
        {
            return Measures.Count == 0;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.OpenBracket("(");

            int i = 0;
            foreach (var measure in Measures)
            {
                dc.BeginLine();

                measure.Draw(dc);
                dc.Append(i < Measures.Count - 1 ? "," : "");

                dc.EndOfLine();
                i++;
            }

            dc.CloseBracket(")");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return Measures;
        }

        public void RemoveChild(IMdxElement child)
        {
            Measures.Remove(child);
        }

        public void ReplaceChild(string name, IMdxElement newChildren)
        {
            for (var i = 0; i < Measures.Count; i++)
            {
                var measure = Measures[i];

                if (measure.GetType() != typeof(MdxHierarchy))
                {
                    continue;
                }

                if (((MdxHierarchy)measure).Name == name)
                {
                    Measures[i] = newChildren;
                }
            }
        }
    }
}