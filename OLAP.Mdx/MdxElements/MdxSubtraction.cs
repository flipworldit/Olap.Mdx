using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxSubtraction : IMdxElement
    {
        private IMdxElement _firstMeasure;
        private IMdxElement _secondMeasure;

        public MdxSubtraction(IMdxElement firstMeasure, IMdxElement secondMeasure)
        {
            _firstMeasure = firstMeasure;
            _secondMeasure = secondMeasure;
        }

        public MdxSubtraction(MdxMeasureElement firstMeasure, MdxMeasureElement secondMeasure)
        {
            _firstMeasure = firstMeasure;
            _secondMeasure = secondMeasure;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("(");

            _firstMeasure.Draw(dc);

            dc.Append(" - ");

            _secondMeasure.Draw(dc);

            dc.Append(")");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }
    }
}