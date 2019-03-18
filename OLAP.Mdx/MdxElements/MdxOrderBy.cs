using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxOrderBy : IMdxElement
    {
        private readonly List<IMdxElement> _rows;
        internal readonly string _measureOrDimension;
        internal readonly string _dir;

        public MdxOrderBy(List<IMdxElement> rows, string measureOrDimension, string dir)
        {
            _rows = rows;
            _measureOrDimension = measureOrDimension;
            _dir = dir;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.OpenBracket("NON EMPTY(");
            int i = 0;
            foreach (var column in _rows)
            {
                column.Draw(dc);
                dc.Append(i < _rows.Count - 1 ? "," : "");
                dc.EndOfLine();

                i++;
            }
            dc.Append(",");

            dc.EndOfLine();

            dc.AppendLine("ORDER");
            dc.OpenBracket("(");

            dc.BeginLine();
            
            dc.Append("(" + _measureOrDimension + ".children),");
            dc.EndOfLine();
            dc.BeginLine();
            dc.Append(_measureOrDimension);
            dc.Append(".CurrentMember.Properties(\"Key\"),");
            dc.EndOfLine();
            dc.BeginLine();
            dc.Append(_dir);
            dc.EndOfLine();

            dc.CloseBracket(")");
            dc.EndOfLine();
            dc.CloseBracket(")");


            //, [Measures].[Amount]
            //[Ent Contacts].[Ent MainManagers - Name].CurrentMember.Properties("Key")
            //, asc)
       
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return _rows;
        }
    }
}