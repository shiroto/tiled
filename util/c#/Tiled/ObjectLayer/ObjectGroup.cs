using System.Collections.Generic;
using System.Text;

namespace Tiled.ObjectLayer
{
    public struct ObjectGroup
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<TiledObject> Objects { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("ObjectGroup");
            sb.Append("[");
            sb.Append("name=" + Name);
            sb.Append(",width=" + Width);
            sb.Append(",height=" + Height);
            sb.Append(",objects=" + Objects.ElementsToString());
            sb.Append(",properties=" + Properties.ElementsToString());
            sb.Append("]");
            return sb.ToString();
        }
    }
}
