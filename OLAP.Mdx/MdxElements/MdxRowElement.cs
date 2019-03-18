using System;
using System.Collections.Generic;
using System.Linq;
using LinqExtIn;

namespace OLAP.Mdx.MdxElements
{
    public class MdxRowElement
        : IMdxElement, IMdxCollectionElements
    {
        public List<IMdxElement> _rows = new List<IMdxElement>();

        public MdxRowElement Set(params IMdxElement[] rows)
        {
            _rows.AddRange(rows);

            return this;
        }

        public MdxRowElement Set(IEnumerable<IMdxElement> rows)
        {
            _rows.AddRange(rows);

            return this;
        }

        public void Draw(MdxDrawContext dc)
        {
            if (IsEmpty())
                return;

            if (_rows.Count > 1)
            {
                var unionMdxBuilder = new TypedMdxElement(_rows.ToArray());

                _rows = new List<IMdxElement>();
                _rows.Add(unionMdxBuilder);
            }

            int i = 0;
            foreach (var column in _rows)
            {
                dc.BeginLine();

                column.Draw(dc);
                dc.Append(i < _rows.Count - 1 ? "," : "");

                dc.EndOfLine();
                i++;
            }

            dc.AppendLine("ON ROWS");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return _rows;
        }

        public void OrderBy(string measureOrDimension, string dir)
        {
            var rows = _rows;

            _rows = new List<IMdxElement>()
            {
                new MdxOrderBy(rows, measureOrDimension, dir)
            };
        }

        public void RemoveChild(IMdxElement child)
        {
            _rows.Remove(child);
        }

        public void ReplaceChild(string name, IMdxElement newChildren)
        {
            throw new NotImplementedException();
        }

        public IMdxCollectionElements AddRange(IEnumerable<IMdxElement> measures)
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty()
        {
            return _rows.Count == 0;
        }

        public MdxHierarchy[] RemoveLast()
        {
            if (IsEmpty())
            {
                return null;
            }

            var hierarhiesList = FindHierarchies().ToArray();

            MdxHierarchy[] removeDimensions = null;

            if (hierarhiesList.Length > 0)
            {
                var lastElement = hierarhiesList.Last();

                removeDimensions = hierarhiesList
                    .Where(el => el.Name == lastElement.Name)
                    .ToArray();

                this.Remove(removeDimensions);
            }

            return removeDimensions;
        }
        //TODO: сделать internal
        public IEnumerable<MdxHierarchy> FindHierarchies()
        {
            var allHierarchies = LinqExtendedDescendans.DescendansAllLayerDFS(
                    this as IMdxElement,
                    element => element is MdxHierarchy,
                    element => element.GetChildren())
                .Cast<MdxHierarchy>()
                .Reverse();

            return allHierarchies;
        }
    }
}