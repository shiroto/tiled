using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Tiled.TileLayer
{
    public class Layer
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private LayerData data;
        public int[,] GlobalTileID { get { return data.globalTileID; } }
        public SpriteEffects[,] TileRotation { get { return data.tileRotation; } set { data.tileRotation = value; } }
        public Dictionary<string, string> Properties { get; set; }
        public bool IsDrawn { get; set; }

        internal Texture2D Texture { get; set; }
        public bool IsPrerendered { get; internal set; }

        public Layer()
        {
            IsDrawn = true;
        }

        public void SetDataString(string datastring, string encoding, string compression)
        {
            data = new LayerData(datastring, encoding, compression, this);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Layer");
            sb.Append("[");
            sb.Append("name=" + Name);
            sb.Append(",width=" + Width);
            sb.Append(",height=" + Height);
            sb.Append(",properties=" + Properties.ElementsToString());
            sb.Append("]");
            return sb.ToString();
        }
    }
}
