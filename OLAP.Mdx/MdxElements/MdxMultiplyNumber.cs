using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxMultiplyNumber : IMdxElement
    {
        private IMdxElement _measure;
        private decimal _number;

        public MdxMultiplyNumber(IMdxElement measure, decimal number)
        {
            _measure = measure;
            _number = number;
        }

        public void Draw(MdxDrawContext dc)
        {
            _measure.Draw(dc);

            dc.Append(string.Format(" * {0}", _number));
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }

    }
}