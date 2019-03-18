using System.Collections.Generic;

namespace OLAP.Mdx.MdxElements
{
    public class MdxNonEmptyJoined : IMdxElement
    {
        private List<IMdxElement> _mdxBuilders;

        private List<MdxMeasureElement> _mdxMeasures;

        public MdxNonEmptyJoined(params IMdxElement[] mdxElements)
        {
            _mdxBuilders = new List<IMdxElement>(mdxElements);
        }

        public MdxNonEmptyJoined(IEnumerable<IMdxElement> mdxElements)
        {
            _mdxBuilders = new List<IMdxElement>(mdxElements);
        }

        public MdxNonEmptyJoined(IEnumerable<IMdxElement> mdxElements, IEnumerable<MdxMeasureElement> mdxMeasures)
        {
            _mdxBuilders = new List<IMdxElement>(mdxElements);
            _mdxMeasures = new List<MdxMeasureElement>(mdxMeasures);
        }

        public MdxNonEmptyJoined(MdxHierarchy mdxElements, List<MdxMeasureElement> mdxMeasures)
        {
            _mdxBuilders = new List<IMdxElement> { mdxElements };
            _mdxMeasures = new List<MdxMeasureElement>(mdxMeasures);
        }

        public void Draw(MdxDrawContext dc)
        {
            dc.Append("NONEMPTY ");
            dc.OpenBracket("(");

            int i = 0;
            foreach (var mdxBuilder in _mdxBuilders)
            {
                mdxBuilder.Draw(dc);
                dc.Append(i < _mdxBuilders.Count - 1 ? "," : "");

                i++;
            }

            if (_mdxMeasures != null)
            {
                dc.Append(", ");
                dc.OpenBracket();

                i = 0;
                foreach (var mdxMeasure in _mdxMeasures)
                {
                    mdxMeasure.Draw(dc);
                    dc.Append(i < _mdxMeasures.Count - 1 ? "," : "");
                    dc.EndOfLine();

                    i++;
                }

                dc.CloseBracket();
            }

            dc.CloseBracket(")");
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return _mdxBuilders;
        }
    }
}