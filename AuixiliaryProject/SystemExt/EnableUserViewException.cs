using System;

namespace SystemExt
{
    // <summary>
    // Сводное описание для EnableUserViewException
    // </summary>
    public class EnableUserViewException
        :Exception
    {
        public EnableUserViewException()
            :base()
        {
        }

        public EnableUserViewException(string message)
            : base(message)
        {
        }

        public EnableUserViewException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}