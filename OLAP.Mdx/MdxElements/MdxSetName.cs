using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxSetName : IMdxElement
    {
        private string _name;

        public MdxSetName(string name)
        {
            _name = name;
        }
        
        public void Draw(MdxDrawContext dc)
        {
            dc.Append(string.Format("[{0}]", _name));
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }
    }
}