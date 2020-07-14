using System;

namespace MinorLinq.Lib 
{
    public class MinorLinqTranslateException : Exception
    {
        public MinorLinqTranslateException() { }
        public MinorLinqTranslateException(string message) : base(message) { }
        public MinorLinqTranslateException(string message, Exception inner) : base(message, inner) { }
    }
    
    public class MinorLinqContextNotConfiguredException : Exception
    {
        public MinorLinqContextNotConfiguredException() { }
        public MinorLinqContextNotConfiguredException(string message) : base(message) { }
        public MinorLinqContextNotConfiguredException(string message, Exception inner) : base(message, inner) { }
    }
    
    public class MinorLinqInvalidExpressionTypeException : Exception
    {
        public MinorLinqInvalidExpressionTypeException() { }
        public MinorLinqInvalidExpressionTypeException(string message) : base(message) { }
        public MinorLinqInvalidExpressionTypeException(string message, Exception inner) : base(message, inner) { }
    }
    
    public class MinorLinqInvalidReaderRowException : Exception
    {
        public MinorLinqInvalidReaderRowException() { }
        public MinorLinqInvalidReaderRowException(string message) : base(message) { }
        public MinorLinqInvalidReaderRowException(string message, Exception inner) : base(message, inner) { }
    }
    
    public class MinorLinqSqlBuilderInvalidGenerateOrder : Exception
    {
        public MinorLinqSqlBuilderInvalidGenerateOrder() { }
        public MinorLinqSqlBuilderInvalidGenerateOrder(string message) : base(message) { }
        public MinorLinqSqlBuilderInvalidGenerateOrder(string message, Exception inner) : base(message, inner) { }
    }
}