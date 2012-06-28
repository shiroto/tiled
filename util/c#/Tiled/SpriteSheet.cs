using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Tiled
{
    public class SpriteSheet
    {
        public int TilesAcross { get; set; }
        public int TilesDown { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int TileSpacing { get; set; }
        public int TileMargin { get; set; }
        private int width, height;
        public int Width
        {
            get { return texture == null ? width : texture.Width; }
            set
            {
                width = value;
                CalculateTilesAcross();
                CalculateTilesDown();
            }
        }
        public int Height
        {
            get { return texture == null ? height : texture.Height; }
            set
            {
                height = value;
                CalculateTilesAcross();
                CalculateTilesDown();
            }
        }

        private Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                CalculateTilesAcross();
                CalculateTilesDown();
            }
        }

        public SpriteSheet() { }

        public SpriteSheet(Texture2D texture, int tileWidth, int tileHeight)
        {
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;
            Texture = texture;
        }

        public Point GetTile(int id)
        {
            return new Point(id % TilesAcross, id / TilesAcross);
        }

        private void CalculateTilesAcross()
        {
            TilesAcross = Width / TileWidth;
        }

        private void CalculateTilesDown()
        {
            TilesDown = Height / TileHeight;
        }
    }
}
