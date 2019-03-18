using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxDivide : IMdxElement
    {
        private IMdxElement _numeratorMeasure;
        private IMdxElement _denominatorMeasure;
        private double _defaultResult;

        public MdxDivide(IMdxElement numeratorMeasure, IMdxElement denominatorMeasure)
        {
            _numeratorMeasure = numeratorMeasure;
            _denominatorMeasure = denominatorMeasure;
        }

        public MdxDivide(MdxMeasureElement numeratorMeasure, MdxMeasureElement denominatorMeasure,double defaultResult=0)
        {
            _numeratorMeasure = numeratorMeasure;
            _denominatorMeasure = denominatorMeasure;
            _defaultResult = defaultResult;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("Divide (");

            _numeratorMeasure.Draw(dc);

            dc.Append(", ");

            _denominatorMeasure.Draw(dc);

            dc.Append(", ");
            dc.Append(_defaultResult+")");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }

    }
}