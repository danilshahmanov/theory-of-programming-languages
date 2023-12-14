using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheoryOfProgrammingLanguages.Source.Exceptions
{
    internal class UnexpectedEndOfProgramException : Exception
    {
        public UnexpectedEndOfProgramException() : base() { }
        public UnexpectedEndOfProgramException(string message) : base(message) { }
        public UnexpectedEndOfProgramException(string message, Exception innerException) : base(message, innerException) { }
    }
}
