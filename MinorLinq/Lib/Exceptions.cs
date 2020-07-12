using System;

namespace MinorLinq.Lib 
{
    public class MinorLinqTranslateException : Exception
    {
        public MinorLinqTranslateException()
        {
        }

        public MinorLinqTranslateException(string message)
            : base(message)
        {
        }

        public MinorLinqTranslateException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}