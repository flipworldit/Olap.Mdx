using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxAvg : IMdxElement
    {
        private IMdxElement _dimension;
        private IMdxElement _measure;

        public MdxAvg(IMdxElement dimension, MdxMeasureElement measure)
            : this(dimension, (IMdxElement)measure)
        {

        }

        public MdxAvg(IMdxElement dimension, IMdxElement measure)
        {
            _dimension = dimension;
            _measure = measure;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("divide (");
            dc.Append("Sum (");

            _dimension.Draw(dc);

            dc.Append(", ");

            _measure.Draw(dc);

            dc.Append(")");

            dc.Append(",Count(");
            _dimension.Draw(dc);

            dc.Append("))");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }
    }
}
