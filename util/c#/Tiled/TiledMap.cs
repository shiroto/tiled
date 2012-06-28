using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

using Tiled.TileLayer;
using Tiled.ObjectLayer;
using Tiled.Exceptions;

/*
 * Author: Marco Knietzsch
 * Since: 2011
 * Version: 0.62
 */
namespace Tiled
{
    public class TiledMap
    {
        public const int ORTHOGONAL = 1, ISOMETRIC = 2;

        public string Version { get; set; }
        public int Orientation { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int TotalWidth { get { return Width * TileWidth; } }
        public int TotalHeight { get { return Height * TileHeight; } }
        public Dictionary<string, string> Properties { get; set; }

        public List<Tileset> Tilesets { get; set; }
        public List<Layer> Layers { get; set; }
        public List<ObjectGroup> ObjectGroups { get; set; }

        public TiledMap()
        {
            Tilesets = new List<Tileset>();
            Layers = new List<Layer>();
            Orientation = ORTHOGONAL;
        }

        public Tileset FindTileset(int globalTileID)
        {
            for (int i = 0; i < Tilesets.Count; i++)
            {
                if (globalTileID < Tilesets[i].LastGID)
                    return Tilesets[i];
                else
                    globalTileID -= Tilesets[i].LastGID;
            }
            throw new TilesetNotFoundException("No Tileset found for ID " + globalTileID + ".");
        }

        public int GetLocalTileID(int globalTileID)
        {
            int localTileID = globalTileID;
            for (int i = 0; i < Tilesets.Count; i++)
            {
                if (localTileID < Tilesets[i].LastGID)
                    return localTileID;
                else
                    localTileID -= Tilesets[i].LastGID;
            }
            throw new TilesetNotFoundException("No Tileset found for ID " + globalTileID + ".");
        }

        public void SetOrientation(string orientation)
        {
            switch (orientation)
            {
                case "orthogonal":
                    Orientation = ORTHOGONAL;
                    break;
                case "isometric":
                    Orientation = ISOMETRIC;
                    break;
                default:
                    throw new Exception("Unsupported orientation");
            }
        }

        public Layer GetLayer(string name)
        {
            foreach (Layer l in Layers)
                if (l.Name.Equals(name))
                    return l;
            throw new Exception("Layer \"" + name + "\" was not found.");
        }

        public bool IsOrthogonal()
        {
            return Orientation == ORTHOGONAL;
        }

        public bool IsIsometric()
        {
            return Orientation == ISOMETRIC;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Map");
            sb.Append("[");
            sb.Append("orientation=" + Orientation);
            sb.Append(",height=" + Height);
            sb.Append(",width=" + Width);
            sb.Append(",tileWidth=" + TileWidth);
            sb.Append(",tileHeight=" + TileHeight);
            sb.Append(",tilesets=" + Tilesets.ElementsToString());
            sb.Append(",layers=" + Layers.ElementsToString());
            sb.Append(",objectgroups=" + ObjectGroups.ElementsToString());
            sb.Append(",properties=" + Properties.ElementsToString());
            sb.Append("]");
            return sb.ToString();
        }
    }
}
