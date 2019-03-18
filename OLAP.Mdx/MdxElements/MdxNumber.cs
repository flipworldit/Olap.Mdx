using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxNumber
        : IMdxElement
    {
        private int _number;

        public MdxNumber(int number)
        {
            _number = number;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append(_number.ToString());
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }
    }
}