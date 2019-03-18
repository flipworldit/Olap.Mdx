using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxMultiply : IMdxElement
    {
        private IMdxElement _firstMeasure;
        private IMdxElement _secondMeasure;

        public MdxMultiply(IMdxElement firstMeasure, IMdxElement secondMeasure)
        {
            _firstMeasure = firstMeasure;
            _secondMeasure = secondMeasure;
        }

        public MdxMultiply(MdxMeasureElement firstMeasure, MdxMeasureElement secondMeasure)
        {
            _firstMeasure = firstMeasure;
            _secondMeasure = secondMeasure;
        }

       public void Draw(MdxDrawContext dc)
        {
            _firstMeasure.Draw(dc);

            dc.Append(" * ");

            _secondMeasure.Draw(dc);
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }

    }
}