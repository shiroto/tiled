using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;


namespace Tiled.TMXProcessorLib
{
    [ContentTypeWriter]
    internal class TMXWriter : ContentTypeWriter<Dictionary<string, string>>
    {
        protected override void Write(ContentWriter output, Dictionary<string, string> value)
        {
            WriteMap(output, value);
            WriteTilesets(output, value);
            WriteLayers(output, value);
            WriteObjectsGroups(output, value);
        }

        private void WriteMap(ContentWriter output, Dictionary<string, string> value)
        {
            WriteMapAttributes(output, value);
            WriteProperties(output, value, "map");
        }

        private void WriteTilesets(ContentWriter output, Dictionary<string, string> value)
        {
            output.Write(Int32.Parse(value["tilesetcount"]));
            for (int i = 0; i < Int32.Parse(value["tilesetcount"]); i++)
            {
                WriteTileset(output, value, i);
            }
        }

        private static void WriteTileset(ContentWriter output, Dictionary<string, string> value, int i)
        {
            output.Write(Int32.Parse((value["tileset" + i + "firstgid"])));
            output.Write(value["tileset" + i + "name"]);
            output.Write(Int32.Parse(value["tileset" + i + "tilewidth"]));
            output.Write(Int32.Parse(value["tileset" + i + "tileheight"]));
            output.Write(Int32.Parse(value["tileset" + i + "spacing"]));
            output.Write(Int32.Parse(value["tileset" + i + "margin"]));

            output.Write(value["tileset" + i + "image" + "source"]);
            if (value.ContainsKey("tileset" + i + "image" + "trans"))
                output.Write(value["tileset" + i + "image" + "trans"]);
            else
                output.Write("no transparency");
            output.Write(Int32.Parse(value["tileset" + i + "image" + "width"]));
            output.Write(Int32.Parse(value["tileset" + i + "image" + "height"]));
        }

        private void WriteLayers(ContentWriter output, Dictionary<string, string> value)
        {
            output.Write(Int32.Parse(value["layercount"]));
            for (int i = 0; i < Int32.Parse(value["layercount"]); i++)
            {
                WriteLayer(output, value, i);
            }
        }

        private static void WriteLayer(ContentWriter output, Dictionary<string, string> value, int i)
        {
            WriteLayerAttributes(output, value, i);
            WriteProperties(output, value, "layer" + i);
            output.Write(value["layer" + i + "encoding"]);
            output.Write(value["layer" + i + "compression"]);
            output.Write(value["layer" + i + "data"]);
        }

        private void WriteObjectsGroups(ContentWriter output, Dictionary<string, string> value)
        {
            output.Write(Int32.Parse(value["objectgroupcount"]));
            for (int i = 0; i < Int32.Parse(value["objectgroupcount"]); i++)
            {
                WriteObjectGroup(output, value, i);
            }
        }

        private static void WriteObjectGroup(ContentWriter output, Dictionary<string, string> value, int i)
        {
            WriteObjectGroupAttributes(output, value, i);
            WriteProperties(output, value, "objectgroup" + i);
            WriteObjects(output, value, i);
        }

        private static void WriteObjects(ContentWriter output, Dictionary<string, string> value, int i)
        {
            output.Write(Int32.Parse(value["objectgroup" + i + "objectcount"]));
            for (int j = 0; j < Int32.Parse(value["objectgroup" + i + "objectcount"]); j++)
            {
                WriteObject(output, value, i, j);
            }
        }

        private static void WriteObject(ContentWriter output, Dictionary<string, string> value, int i, int j)
        {
            output.Write(value["objectgroup" + i + "object" + j + "name"]);
            output.Write(value["objectgroup" + i + "object" + j + "type"]);
            output.Write(Int32.Parse(value["objectgroup" + i + "object" + j + "x"]));
            output.Write(Int32.Parse(value["objectgroup" + i + "object" + j + "y"]));
            output.Write(Int32.Parse(value["objectgroup" + i + "object" + j + "width"]));
            output.Write(Int32.Parse(value["objectgroup" + i + "object" + j + "height"]));
            WriteProperties(output, value, "objectgroup" + i + "object" + j);
        }

        private static void WriteMapAttributes(ContentWriter output, Dictionary<string, string> value)
        {
            output.Write(value["mapversion"]);
            output.Write(value["maporientation"]);
            output.Write(Int32.Parse(value["mapwidth"]));
            output.Write(Int32.Parse(value["mapheight"]));
            output.Write(Int32.Parse(value["maptilewidth"]));
            output.Write(Int32.Parse(value["maptileheight"]));
        }

        private static void WriteLayerAttributes(ContentWriter output, Dictionary<string, string> value, int i)
        {
            output.Write(value["layer" + i + "name"]);
            output.Write(Int32.Parse(value["layer" + i + "width"]));
            output.Write(Int32.Parse(value["layer" + i + "height"]));
            output.Write(value["layer" + i + "opacity"]);
        }

        private static void WriteObjectGroupAttributes(ContentWriter output, Dictionary<string, string> value, int i)
        {
            output.Write(value["objectgroup" + i + "name"]);
            output.Write(Int32.Parse(value["objectgroup" + i + "width"]));
            output.Write(Int32.Parse(value["objectgroup" + i + "height"]));
        }

        private static void WriteProperties(ContentWriter output, Dictionary<string, string> value, string path)
        {
            output.Write(Int32.Parse(value[path + "propertycount"]));
            int propertyCount = Int32.Parse(value[path + "propertycount"]);
            for (int i = 0; i < propertyCount; i++)
            {
                output.Write(value[path + "property" + i + "name"]);
                output.Write(value[path + "property" + i + "value"]);
            }
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(TiledMap).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Tiled.TMXProcessorLib.TMXReader, Tiled";
        }
    }
}
