using System.Text;


namespace Tiled
{
    public class Tileset : SpriteSheet
    {
        public string Name { get; set; }
        public int LastGID { get { return TilesAcross * TilesDown; } }

        public string ImageName { get; set; }

        public Microsoft.Xna.Framework.Color Transparency { get; set; }

        public Tileset() { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Tileset");
            sb.Append("[");
            sb.Append("name=" + Name);
            sb.Append(",texture=" + Texture);
            sb.Append(",width=" + Width);
            sb.Append(",height=" + Height);
            sb.Append(";image name=" + ImageName);
            sb.Append(",lastGID=" + LastGID);
            sb.Append(",tileWidth=" + TileWidth);
            sb.Append(",tileHeight=" + TileHeight);
            sb.Append(",tilesAcross=" + TilesAcross);
            sb.Append(",tilesDown=" + TilesDown);
            sb.Append(",tileSpacing=" + TileSpacing);
            sb.Append(",tileMargin=" + TileMargin);
            sb.Append("]");
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Tileset)
                return Equals((Tileset)obj);
            else
                return false;
        }

        public bool Equals(Tileset tileset)
        {
            return this.Name.Equals(tileset.Name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
