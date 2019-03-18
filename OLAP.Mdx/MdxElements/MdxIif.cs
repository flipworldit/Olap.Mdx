using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxIif : IMdxElement
    {
        private readonly IMdxElement _iifExpression;
        private readonly IMdxElement _trueValue;
        private readonly IMdxElement _falseValue;

        public MdxIif(IMdxElement iifExpression, IMdxElement trueValue, IMdxElement falseValue)
        {
            _iifExpression = iifExpression;
            _trueValue = trueValue;
            _falseValue = falseValue;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("IIF (");

            _iifExpression.Draw(dc);

            dc.Append(", ");

            _trueValue.Draw(dc);

            dc.Append(", ");

            _falseValue.Draw(dc);

            dc.Append(")");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            throw new System.NotImplementedException();
        }

    }
}