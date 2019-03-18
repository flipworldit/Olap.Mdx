using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxMeasureIsNullElement : IMdxElement
    {
        private IMdxElement _measure;

        public MdxMeasureIsNullElement(IMdxElement measure)
        {
            _measure = measure;
        }

        public void Draw(MdxDrawContext dc)
        {
            _measure.Draw(dc);

            dc.Append(" IS NULL");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }

    }
}