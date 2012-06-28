using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiled.Exceptions
{
    public sealed class TilesetNotFoundException : Exception
    {
        public TilesetNotFoundException(string message) : base(message) { }
    }
}
