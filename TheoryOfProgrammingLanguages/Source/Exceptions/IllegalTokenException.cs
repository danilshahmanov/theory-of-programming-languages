using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheoryOfProgrammingLanguages.Source.Exceptions
{
    internal class IllegalTokenException : Exception
    {
        public IllegalTokenException() : base() { }
        public IllegalTokenException(string message) : base(message) { }
        public IllegalTokenException(string message, Exception innerException) : base(message, innerException) { }
    }
}
