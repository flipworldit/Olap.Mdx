using System.Collections.Generic;
using System.Linq;

namespace OLAP.Mdx.MdxElements
{
    public class MdxMeasureElement
        : IMdxElement
    {
        public string Measure { get; set; }

        private bool _minus = false;
        private MdxBuilder.ValueFilter _valueFilter = MdxBuilder.ValueFilter.All;

        public MdxMeasureElement()
        {
            
        }

        public static implicit operator MdxMeasureElement(string name)
        {
            return new MdxMeasureElement(name);
        }

        public MdxMeasureElement(string measure)
        {
            Measure = measure;
        }

        public MdxMeasureElement Minus
        {
            get
            {
                _minus = true;

                return this;
            }
        }

        public MdxMeasureElement FilterValues(MdxBuilder.ValueFilter valueFilter)
        {
            _valueFilter = valueFilter;
                
            return this;
        }

        public void Draw(MdxDrawContext dc)
        {
            if (_valueFilter == MdxBuilder.ValueFilter.All)
            {
                DrowMeasure(dc);
             
                return;
            }

            dc.Append("IIF (");

            DrowMeasure(dc);

            var compareChar = _valueFilter == MdxBuilder.ValueFilter.Positive ? ">" : "<";

            dc.Append(string.Format(" {0} 0, ", compareChar));

            DrowMeasure(dc);

            dc.Append(", null)");

        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }

        private void DrowMeasure(MdxDrawContext dc)
        {
            if (_minus)
            {
                dc.Append("-");
            }

            dc.Append("[Measures].[");
            dc.Append(Measure);
            dc.Append("]");
        }
    }
}