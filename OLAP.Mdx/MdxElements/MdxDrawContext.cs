using System.Text;

namespace OLAP.Mdx.MdxElements
{
    public class MdxDrawContext
    {
        private readonly string SPACE = "    ";
        private StringBuilder _sb = new StringBuilder();
        private int _level = 0;

        public void Append(string text)
        {
            _sb.Append(text);
        }

        public void CloseBracket(string bracket = "}")
        {
            _level--;

            BeginLine();

            _sb.Append(bracket);
        }

        public void OpenBracket(string bracket = "{")
        {
            AppendLine(bracket);

            _level++;
        }

        public void AppendLine(string text)
        {
            BeginLine();

            _sb.AppendLine(text);
        }

        public void BeginLine()
        {
            for (int i = 0; i < _level; i++)
            {
                _sb.Append(SPACE);
            }
        }

        public override string ToString()
        {
            return _sb.ToString();
        }

        public void EndOfLine()
        {
            _sb.AppendLine();
        }

        public void IncLevel()
        {
            _level++;
        }

        public void DecLevel()
        {
            _level--;
        }
    }
}