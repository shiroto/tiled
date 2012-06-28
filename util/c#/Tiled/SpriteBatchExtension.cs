using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tiled.TileLayer;
using Tiled.ObjectLayer;
using Tiled.Exceptions;


namespace Tiled
{
    public static class SpriteBatchExtension
    {
        public static void Draw(this SpriteBatch spriteBatch, TiledMap map, Vector2 position)
        {
            spriteBatch.Draw(map, new Rectangle((int)position.X, (int)position.Y, map.TotalWidth, map.TotalHeight));
        }

        public static void Draw(this SpriteBatch spriteBatch, TiledMap map, Rectangle destinationRectangle)
        {
            spriteBatch.Draw(map, 0, 0, map.Width, map.Height, destinationRectangle);
        }

        public static void Draw(this SpriteBatch spriteBatch, TiledMap map, int x, int y, int width, int height, Vector2 position)
        {
            spriteBatch.Draw(map, x, y, width, height, new Rectangle((int)position.X, (int)position.Y, map.TotalWidth, map.TotalHeight));
        }

        public static void Draw(this SpriteBatch spriteBatch, TiledMap map, int x, int y, int width, int height, Rectangle destinationRectangle)
        {
            for (int i = 0; i < map.Layers.Count; i++)
            {
                spriteBatch.Draw(map, map.Layers[i], x, y, width, height, destinationRectangle);
            }
        }

        public static void Draw(this SpriteBatch spriteBatch, TiledMap map, Layer layer, Vector2 position)
        {
            spriteBatch.Draw(map, layer, new Rectangle((int)position.X, (int)position.Y, map.TotalWidth, map.TotalHeight));
        }

        public static void Draw(this SpriteBatch spriteBatch, TiledMap map, Layer layer, Rectangle destinationRectangle)
        {
            spriteBatch.Draw(map, layer, 0, 0, layer.GlobalTileID.GetLength(0), layer.GlobalTileID.GetLength(1), destinationRectangle);
        }

        public static void Draw(this SpriteBatch spriteBatch, TiledMap map, Layer layer, int x, int y, int width, int height, Vector2 position)
        {
            spriteBatch.Draw(map, layer, 0, 0, layer.GlobalTileID.GetLength(0), layer.GlobalTileID.GetLength(1), new Rectangle((int)position.X, (int)position.Y, map.TotalWidth, map.TotalHeight));
        }

        public static void Draw(this SpriteBatch spriteBatch, TiledMap map, Layer layer, int x, int y, int width, int height, Rectangle destinationRectangle)
        {
            if (layer.IsDrawn)
            {
                if (layer.IsPrerendered)
                    spriteBatch.Draw(layer.Texture, destinationRectangle, Color.White);
                else
                {
                    int drawTileWidth = destinationRectangle.Width / map.Width;
                    int drawTileHeight = destinationRectangle.Height / map.Height;
                    for (int X = x; X < width; X++)
                    {
                        for (int Y = y; Y < height; Y++)
                        {
                            DrawTile(spriteBatch, map, layer, destinationRectangle, drawTileWidth, drawTileHeight, X, Y);
                        }
                    }
                }
            }
        }

        private static void DrawTile(SpriteBatch spriteBatch, TiledMap map, Layer layer, Rectangle destinationRectangle, int drawTileWidth, int drawTileHeight, int X, int Y)
        {
            if (layer.GlobalTileID[X, Y] >= 0) // negative value indicates that theres no tile to be painted
            {
                try
                {
                    switch (map.Orientation)
                    {
                        case TiledMap.ORTHOGONAL:
                            DrawTileOrthogonal(spriteBatch, map, layer, destinationRectangle, drawTileWidth, drawTileHeight, X, Y);
                            break;
                        case TiledMap.ISOMETRIC:
                            DrawTileIsometric(spriteBatch, map, layer, destinationRectangle, drawTileWidth, drawTileHeight, X, Y);
                            break;
                        default:
                            throw new UnsupportedOrientationException("Tried to draw map with unsupported orientation.");
                    }
                }
                catch (TilesetNotFoundException ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private static void DrawTileIsometric(SpriteBatch spriteBatch, TiledMap map, Layer layer, Rectangle destinationRectangle, int drawTileWidth, int drawTileHeight, int X, int Y)
        {
            Tileset tileset = map.FindTileset(layer.GlobalTileID[X, Y]);
            int localTileID = map.GetLocalTileID(layer.GlobalTileID[X, Y]);
            Point tileCoords = tileset.GetTile(localTileID);
            Rectangle sourceRect = new Rectangle(tileCoords.X * tileset.TileWidth, tileCoords.Y * tileset.TileHeight, tileset.TileWidth, tileset.TileHeight);

            int x = (X + map.Width) * drawTileWidth / 2 - drawTileWidth / 2 - Y * drawTileWidth / 2 + destinationRectangle.X;
            int y = Y * drawTileHeight / 2 + X * drawTileHeight / 2 + destinationRectangle.Y;

            Rectangle destinationRect = new Rectangle(x, y, tileset.TileWidth, tileset.TileHeight);

            spriteBatch.Draw(tileset.Texture, destinationRect, sourceRect, Color.White, 0f, new Vector2(), layer.TileRotation[X, Y], 0f);
        }

        private static void DrawTileOrthogonal(SpriteBatch spriteBatch, TiledMap map, Layer layer, Rectangle destinationRectangle, int drawTileWidth, int drawTileHeight, int X, int Y)
        {
            Tileset tileset = map.FindTileset(layer.GlobalTileID[X, Y]);
            int localTileID = map.GetLocalTileID(layer.GlobalTileID[X, Y]);
            Point tileCoords = tileset.GetTile(localTileID);
            Rectangle sourceRect = new Rectangle(tileCoords.X * tileset.TileWidth + tileset.TileSpacing * tileCoords.X, tileCoords.Y * tileset.TileHeight + tileset.TileSpacing * tileCoords.Y, tileset.TileWidth, tileset.TileHeight);
            Rectangle destinationRect = new Rectangle(X * drawTileWidth + destinationRectangle.X, Y * drawTileHeight + destinationRectangle.Y, (int)(tileset.TileWidth * ((float)destinationRectangle.Width / (float)map.TotalWidth)), (int)(tileset.TileHeight * ((float)destinationRectangle.Height / (float)map.TotalHeight)));

            spriteBatch.Draw(tileset.Texture, destinationRect, sourceRect, Color.White, 0f, new Vector2(), layer.TileRotation[X, Y], 0f);
        }

        public static void PrerenderMap(this SpriteBatch spriteBatch, TiledMap map)
        {
            foreach (Layer l in map.Layers)
                spriteBatch.PrerenderLayer(map, l);
        }

        public static void PrerenderLayer(this SpriteBatch spriteBatch, TiledMap map, Layer layer)
        {
            bool layerIsDrawn = layer.IsDrawn;
            layer.IsDrawn = true;
            GraphicsDevice graphics = spriteBatch.GraphicsDevice;
            RenderTargetBinding[] initialRenderTargets = graphics.GetRenderTargets();
            RenderTarget2D renderTarget = new RenderTarget2D(graphics, map.TotalWidth, map.TotalHeight);
            graphics.SetRenderTarget(renderTarget);
            layer.Texture = renderTarget;
            graphics.Clear(Color.Transparent);
            spriteBatch.Begin();
            spriteBatch.Draw(map, layer, Vector2.Zero);
            spriteBatch.End();
            graphics.SetRenderTargets(initialRenderTargets);
            layer.IsDrawn = layerIsDrawn;
            layer.IsPrerendered = true;
        }
    }
}
