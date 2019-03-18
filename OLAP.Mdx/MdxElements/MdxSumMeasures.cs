using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxSumMeasures : IMdxElement
    {
        private string[] _measures;

        public MdxSumMeasures(params string[] measures)
        {
            _measures = measures;
        }

        public void Draw(MdxDrawContext dc)
        {
            for (var i = 0; i < _measures.Length; i++)
            {
                dc.Append("[Measures].[");
                dc.Append(_measures[i]);
                dc.Append("]");

                if (i < (_measures.Length - 1))
                dc.Append(" + ");
            }
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }
    }
}
