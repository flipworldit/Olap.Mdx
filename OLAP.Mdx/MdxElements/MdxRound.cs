using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxRound : IMdxElement
    {
        private IMdxElement _numeratorMeasure;
        private int _digitsCount;

        public MdxRound(IMdxElement numeratorMeasure, int digitsCount)
        {
            _numeratorMeasure = numeratorMeasure;
            _digitsCount = digitsCount;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("Round (");

            _numeratorMeasure.Draw(dc);

            dc.Append(string.Format(", {0})", _digitsCount));
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }
    }
}