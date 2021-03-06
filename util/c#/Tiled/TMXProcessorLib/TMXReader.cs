using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Tiled.TileLayer;
using Tiled.ObjectLayer;
using Tiled.Exceptions;

namespace Tiled.TMXProcessorLib
{
    internal class TMXReader : ContentTypeReader<TiledMap>
    {
        private static ContentReader input;

        protected override TiledMap Read(ContentReader input,
            TiledMap existingInstance)
        {
            TMXReader.input = input;
            TiledMap map = ReadMap();
            map.Tilesets = ReadTilesets();
            map.Layers = ReadLayers();
            map.ObjectGroups = ReadObjectGroups();
            return map;
        }

        private TiledMap ReadMap()
        {
            TiledMap map = new TiledMap();
            map.Version = input.ReadString();
            map.SetOrientation(input.ReadString());
            map.Width = input.ReadInt32();
            map.Height = input.ReadInt32();
            map.TileWidth = input.ReadInt32();
            map.TileHeight = input.ReadInt32();

            map.Properties = ReadProperties();
            return map;
        }

        private List<Tileset> ReadTilesets()
        {
            int tilesetcount = input.ReadInt32();
            List<Tileset> tilesets = new List<Tileset>(tilesetcount);
            for (int i = 0; i < tilesetcount; i++)
                tilesets.Add(ReadTileset());
            return tilesets;
        }

        private static Tileset ReadTileset()
        {
            Tileset t = new Tileset();
            input.ReadInt32(); // first gid, unneeded
            t.Name = input.ReadString();
            t.TileWidth = input.ReadInt32();
            t.TileHeight = input.ReadInt32();
            t.TileSpacing = input.ReadInt32();
            t.TileMargin = input.ReadInt32();

            t.ImageName = ReadTilesetName();
            t.Transparency = ReadTransparency(t);

            t.Width = input.ReadInt32();
            t.Height = input.ReadInt32();

            t = TilesetManager.GetAsListElement(t);
            return t;
        }

        private static string ReadTilesetName()
        {
            string name = input.ReadString();
            string[] cutPath = name.Split('/');
            name = cutPath[cutPath.Length - 1];
            return name.Split('.')[0];
        }

        private static Microsoft.Xna.Framework.Color ReadTransparency(Tileset t)
        {
            Microsoft.Xna.Framework.Color trans = new Microsoft.Xna.Framework.Color();
            string transparency = input.ReadString();
            if (!transparency.Equals("no transparency"))
            {
                string first = transparency[0] + "" + transparency[1];
                string second = transparency[2] + "" + transparency[3];
                string third = transparency[4] + "" + transparency[5];
                int r = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                int g = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);
                int b = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);
                trans = new Microsoft.Xna.Framework.Color(r, g, b);
            }
            return trans;
        }

        private List<Layer> ReadLayers()
        {
            int layercount = input.ReadInt32();
            List<Layer> layers = new List<Layer>(layercount);
            for (int i = 0; i < layercount; i++)
            {
                Layer l = ReadLayer();
                layers.Add(l);
            }
            return layers;
        }

        private static Layer ReadLayer()
        {
            Layer l = new Layer();
            ReadLayerAttributes(ref l);
            l.Properties = ReadProperties();

            string encoding = input.ReadString();
            string compression = input.ReadString();
            string datastring = input.ReadString();
            l.SetDataString(datastring, encoding, compression);
            return l;
        }

        private static void ReadLayerAttributes(ref Layer l)
        {
            l.Name = input.ReadString();
            l.Width = input.ReadInt32();
            l.Height = input.ReadInt32();
            input.ReadString(); // ignore opacity
        }

        private List<ObjectGroup> ReadObjectGroups()
        {
            int objectGroupCount = input.ReadInt32();
            List<ObjectGroup> objectGroups = new List<ObjectGroup>();
            for (int i = 0; i < objectGroupCount; i++)
                objectGroups.Add(ReadObjectGroup(i));
            return objectGroups;
        }

        private ObjectGroup ReadObjectGroup(int objectGroupNumber)
        {
            ObjectGroup og = new ObjectGroup();
            ReadObjectGroupAttribute(ref og);
            og.Properties = ReadProperties();
            og.Objects = ReadObjects(objectGroupNumber, og);
            return og;
        }

        private List<TiledObject> ReadObjects(int objectGroupNumber, ObjectGroup og)
        {
            List<TiledObject> list = new List<TiledObject>();
            int objectCount = input.ReadInt32();
            for (int objectNumber = 0; objectNumber < objectCount; objectNumber++)
                list.Add(ReadObject(objectGroupNumber, objectNumber));
            return list;
        }

        private static void ReadObjectGroupAttribute(ref ObjectGroup og)
        {
            og.Name = input.ReadString();
            og.Width = input.ReadInt32();
            og.Height = input.ReadInt32();
        }

        private TiledObject ReadObject(int objectGroupNumber, int objectNumber)
        {
            TiledObject to = new TiledObject();
            ReadObjectAttribute(ref to);
            to.Properties = ReadProperties();
            return to;
        }

        private static void ReadObjectAttribute(ref TiledObject to)
        {
            to.Name = input.ReadString();
            to.Type = input.ReadString();
            int x = input.ReadInt32();
            int y = input.ReadInt32();
            to.Position = new Vector2(x, y);
            to.Width = input.ReadInt32();
            to.Height = input.ReadInt32();
        }

        private static Dictionary<string, string> ReadProperties()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            int propertyCount = input.ReadInt32();
            for (int i = 0; i < propertyCount; i++)
            {
                string name = input.ReadString();
                string value = input.ReadString();
                dic.Add(name, value);
            }
            return dic;
        }
    }
}
