using System.Collections.Generic;
using System.Linq;

namespace OLAP.Mdx.MdxElements
{
    public class MdxValueElement
        : MdxHierarchy, IMdxElement
    {
        public string Value;
        private string _secondValue;
        private object _lagValue;

        public MdxValueElement(
            MdxHierarchy hierarchyElement, 
            string value,
            string secondValue = null,
            object lagValue = null)
            :base(hierarchyElement)
        {
            Value = value;
            _secondValue = secondValue;
            _lagValue = lagValue;
        }

        public override MdxValueElement Lag(object lagValue)
        {
            _lagValue = lagValue;

            return this;
        }

        public override void Draw(MdxDrawContext dc)
        {
            base.Draw(dc);

            if (Value == "Unknown")
            {
                dc.Append(".[All].UNKNOWNMEMBER");
            }
            else if (Value!= null)
            {
                dc.Append(".&[");
                dc.Append(Value);
                dc.Append("]");
            }

            if (_secondValue != null)
            {

                if (_secondValue == "Unknown")
                {
                    dc.Append(".[All].UNKNOWNMEMBER");
                }
                else
                {
                    dc.Append("&[");
                    dc.Append(_secondValue);
                    dc.Append("]");
                }
            }

            if (_lagValue != null)
            {
                dc.Append(string.Format(".Lag({0})", _lagValue));
            }
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new IMdxElement[]{};
        }
    }
}