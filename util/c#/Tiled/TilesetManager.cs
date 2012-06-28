using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tiled.Exceptions;


namespace Tiled
{
    public static class TilesetManager
    {
        private static readonly List<Tileset> tilesets = new List<Tileset>();

        public static void Add(Tileset tileset)
        {
            tilesets.Add(tileset);
        }

        public static bool Contains(string tileset)
        {
            foreach (Tileset t in tilesets)
            {
                if (tileset.Equals(t.Name))
                    return true;
            }
            return false;
        }

        public static int Count
        {
            get { return tilesets.Count; }
        }

        public static Tileset Get(int index)
        {
            return tilesets[index];
        }

        public static Tileset Get(string name)
        {
            for (int i = 0; i < tilesets.Count; i++)
            {
                if (tilesets[i].Name.Equals(name))
                    return tilesets[i];
            }
            throw new TilesetNotFoundException("Tileset was not found.");
        }

        public static Tileset GetAsListElement(Tileset tileset)
        {
            if (Contains(tileset.Name))
                return tilesets[tilesets.IndexOf(tileset)];
            else
            {
                tilesets.Add(tileset);
                return tileset;
            }
        }

        public static List<Tileset>.Enumerator GetEnumerator()
        {
            return tilesets.GetEnumerator();
        }

        public static int IndexOf(string tileset)
        {
            for (int i = 0; i < tilesets.Count; i++)
            {
                if (tilesets[i].Name.Equals(tileset))
                    return i;
            }
            throw new TilesetNotFoundException("Tileset was not found.");
        }

        public static void LoadTextures(ContentManager content)
        {
            foreach (Tileset t in tilesets)
            {
                t.Texture = content.Load<Texture2D>(t.ImageName);
                ApplyTransparency(t);
            }
        }

        public static void Remove(Tileset tileset)
        {
            tilesets.Remove(tileset);
        }

        public static Tileset[] ToArray()
        {
            return tilesets.ToArray();
        }

        public static void ApplyTransparency(Tileset tileset)
        {
            Color[] pixel = new Color[tileset.Width * tileset.Height];
            tileset.Texture.GetData<Color>(pixel);
            for (int i = 0; i < pixel.Count(); i++)
            {
                if (pixel[i].Equals(tileset.Transparency))
                    pixel[i] = new Color();
            }
            tileset.Texture.SetData<Color>(pixel);
        }
    }
}
