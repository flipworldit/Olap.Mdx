using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxFilterElement : IMdxElement
    {
        private IMdxElement _mdxSetExpression;
        private IMdxElement _mdxLogicalExpression;


        public MdxFilterElement(IMdxElement mdxSetExpression, IMdxElement mdxLogicalExpression)
        {
            _mdxSetExpression = mdxSetExpression;
            _mdxLogicalExpression = mdxLogicalExpression;
        }


        public void Draw(MdxDrawContext dc)
        {
            dc.Append("Filter (");

            _mdxSetExpression.Draw(dc);

            dc.Append(", ");

            _mdxLogicalExpression.Draw(dc);

            dc.Append(")");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }


    }
}
