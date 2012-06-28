using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiled.Exceptions
{
    public class UnsupportedEncodingException : Exception
    {
        public UnsupportedEncodingException(string message) : base(message) { }
    }
}
