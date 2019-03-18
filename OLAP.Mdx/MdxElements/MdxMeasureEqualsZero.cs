using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxMeasureEqualsZero
        : IMdxElement
    {
        private string _measure;

        public MdxMeasureEqualsZero(string measure)
        {
            _measure = measure;
        }

        public void Draw(MdxDrawContext dc)
        {
                DrowMeasure(dc);
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }

        private void DrowMeasure(MdxDrawContext dc)
        {
            dc.Append("[Measures].[");
            dc.Append(_measure);
            dc.Append("] = 0");
        }
    }
}