using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheoryOfProgrammingLanguages.Source.Exceptions
{
    internal class InvalidVariableException : Exception
    {
        public InvalidVariableException() : base() { }
        public InvalidVariableException(string message) : base(message) { }
        public InvalidVariableException(string message, Exception innerException) : base(message, innerException) { }
    }
}
