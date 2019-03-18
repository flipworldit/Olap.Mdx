using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxMember : IMdxElement
    {
        private IMdxElement _asMember;
        private string _name;

        public MdxMember(string name, IMdxElement asMemberMember)
        {
            _name = name;
            _asMember = asMemberMember;
        }

        public MdxMember(string name, MdxMeasureElement asMemberMember)
        {
            _name = name;
            _asMember = asMemberMember;
        }
        
        public void Draw(MdxDrawContext dc)
        {
            dc.Append("MEMBER ");
            dc.Append("[Measures].[");
            dc.Append(_name);
            dc.Append("] AS");

            dc.EndOfLine();
            dc.IncLevel();

            dc.BeginLine();
            _asMember.Draw(dc);

            dc.DecLevel();
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>
            {
                _asMember
            };
        }
    }
}