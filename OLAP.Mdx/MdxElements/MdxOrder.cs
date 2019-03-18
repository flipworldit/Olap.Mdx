using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxOrder : IMdxElement
    {
        private IMdxElement _mdxRowsElement;
        private IMdxElement _measureOrDimension;
        private string _dir;

        public MdxOrder(IMdxElement mdxRowsElement, IMdxElement measureOrDimension, string dir)
        {
            _dir = dir;
            _mdxRowsElement = mdxRowsElement;
            _measureOrDimension = measureOrDimension;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("Order (");

            _mdxRowsElement.Draw(dc);

            dc.Append(", ");

            _measureOrDimension.Draw(dc);

            dc.Append(",");
            dc.Append(_dir);
            dc.Append(")");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }
    }
}