using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxColumnElement
        : IMdxElement
    {
        private List<IMdxElement> _columns = new List<IMdxElement>();

        public MdxColumnElement Set(params IMdxElement[] columns)
        {
            _columns.AddRange(columns);

            return this;
        }

        public MdxColumnElement Set(IEnumerable<IMdxElement> columns)
        {
            _columns.AddRange(columns);

            return this;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.IncLevel();

            int i = 0;
            foreach (var column in _columns)
            {
                column.Draw(dc);
                dc.Append(i < _columns.Count - 1 ? "," : "");

                dc.EndOfLine();
                i++;
            }

            dc.DecLevel();

            dc.Append("ON COLUMNS");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return _columns;
        }
    }
}