using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tiled.ObjectLayer
{
    public struct TiledObject
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("TiledObject");
            sb.Append("[");
            sb.Append("name=" + Name);
            sb.Append(",x=" + Position.X);
            sb.Append(",y=" + Position.Y);
            sb.Append(",width=" + Width);
            sb.Append(",height=" + Height);
            sb.Append(",properties=" + Properties.ElementsToString());
            sb.Append("]");
            return sb.ToString();
        }
    }
}
