using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxWithElement
        : IMdxElement
    {
        private List<IMdxElement> _members = new List<IMdxElement>();

        public MdxWithElement Set(params IMdxElement[] members)
        {
            _members.AddRange(members);

            return this;
        }

        public MdxWithElement Set(IEnumerable<IMdxElement> members)
        {
            _members.AddRange(members);

            return this;
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.AppendLine("WITH");
            dc.IncLevel();

            int i = 0;
            foreach (var measure in _members)
            {
                dc.BeginLine();

                measure.Draw(dc);

                dc.EndOfLine();
                i++;
            }

            dc.DecLevel();
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return _members;
        }
    }
}
