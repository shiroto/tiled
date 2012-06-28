using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiled.Exceptions
{
    public class UnsupportedOrientationException : Exception
    {
        public UnsupportedOrientationException(string message) : base(message) { }
    }
}
