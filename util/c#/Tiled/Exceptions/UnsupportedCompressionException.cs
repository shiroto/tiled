using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiled.Exceptions
{
    public class UnsupportedCompressionException : Exception
    {
        public UnsupportedCompressionException(string message) : base(message) { }
    }
}
