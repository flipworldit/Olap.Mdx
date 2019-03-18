using System;
using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxMovAvg : IMdxElement
    {
        private IMdxElement _mdxElement;
        private IMdxElement _measure;
        private int _offset;

        public MdxMovAvg(IMdxElement mdxElement, IMdxElement measure, int offset)
        {
            _mdxElement = mdxElement;
            _measure = measure;
            _offset = offset;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("Sum (");
            dc.EndOfLine();
            dc.IncLevel();
            dc.BeginLine();

            _mdxElement.Draw(dc);

            dc.Append(string.Format(".Lag({0}) :", _offset-1));

            dc.EndOfLine();
            dc.BeginLine();

            _mdxElement.Draw(dc);

            dc.Append(".Lag(0),");

            dc.EndOfLine();
            dc.BeginLine();

            _measure.Draw(dc);

            dc.EndOfLine();
            dc.CloseBracket(")");
            dc.Append("/");
            dc.Append(_offset.ToString());
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }
    }
}