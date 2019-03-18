using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxTopCountElement : IMdxElement
    {
        private IMdxElement _setExpression;
        private IMdxElement _measure;
        private int _count;

        public MdxTopCountElement(IMdxElement setExpression, int count, IMdxElement measure=null)
        {
            _setExpression = setExpression;
            _measure = measure;
            _count = count;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("TopCount (");

            _setExpression.Draw(dc);

            dc.Append(string.Format(", {0}", _count));

            if (_measure != null)
            {
                dc.Append(", ");
                _measure.Draw(dc);
            }

            dc.Append(")");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement> { _setExpression };
        }
    }
}