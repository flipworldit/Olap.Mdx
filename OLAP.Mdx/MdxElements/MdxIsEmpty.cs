using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    
    public class MdxIsEmpty : IMdxElement
    {
        private IMdxElement _mdxValueExpression;

        public MdxIsEmpty(IMdxElement mdxValueExpression)
        {
            _mdxValueExpression = mdxValueExpression;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("IsEmpty(");

            _mdxValueExpression.Draw(dc);

            dc.Append(")");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }
    }
}





