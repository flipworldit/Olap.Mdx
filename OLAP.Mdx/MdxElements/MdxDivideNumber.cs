using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxDivideNumber : IMdxElement
    {
        private IMdxElement _numeratorMeasure;
        private decimal _number;

        public MdxDivideNumber(IMdxElement numeratorMeasure, decimal number)
        {
            _numeratorMeasure = numeratorMeasure;
            _number = number;
        }

        public MdxDivideNumber(MdxMeasureElement numeratorMeasure, decimal number)
        {
            _numeratorMeasure = numeratorMeasure;
            _number = number;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("Divide (");

            _numeratorMeasure.Draw(dc);

            dc.Append(string.Format(", {0}, 0)", _number));
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }

        
    }
}