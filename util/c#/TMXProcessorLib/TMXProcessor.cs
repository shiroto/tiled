using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;


namespace Tiled.TMXProcessorLib
{
    [ContentProcessor(DisplayName = "TMX Processor")]
    internal class TMXProcessor : ContentProcessor<XDocument, Dictionary<string, string>>
    {
        private XDocument tmxFile;
        private Dictionary<string, string> mapProperties;

        public override Dictionary<string, string> Process(XDocument input,
            ContentProcessorContext context)
        {
            mapProperties = new Dictionary<string, string>();
            tmxFile = input;

            ReadMap();
            ReadTilesets();
            ReadLayers();
            ReadObjectGroups();

            return mapProperties;
        }

        private void ReadMap()
        {
            ReadAttributes(tmxFile.Descendants("map").First(), "map");
            ReadProperties(GetPropertiesNode(tmxFile.Descendants("map").First()), "map");
        }

        private void ReadTilesets()
        {
            var tilesetsQuery = from nodes in tmxFile.Descendants("tileset") select nodes;
            mapProperties.Add("tilesetcount", tilesetsQuery.Count() + "");

            int i = 0;
            foreach (XElement tileset in tilesetsQuery)
            {
                ReadTileset(tileset, i);
                i++;
            }
        }

        private void ReadLayers()
        {
            var layersQuery = from nodes in tmxFile.Descendants("layer") select nodes;
            mapProperties.Add("layercount", layersQuery.Count() + "");

            int index = 0;
            foreach (XElement layer in layersQuery)
            {
                ReadLayer(layer, index);
                index++;
            }
        }

        private void ReadObjectGroups()
        {
            var objectGroupQuery = from nodes in tmxFile.Descendants("objectgroup") select nodes;
            mapProperties.Add("objectgroupcount", objectGroupQuery.Count() + "");

            int index = 0;
            foreach (XElement objectGroupElement in objectGroupQuery)
            {
                ReadObjectGroup(objectGroupElement, index);
                index++;
            }
        }

        private void ReadTileset(XElement tilesetElement, int index)
        {
            ReadAttributes(tilesetElement, "tileset" + index);
            if (!mapProperties.ContainsKey("tileset" + index + "spacing"))
                mapProperties.Add("tileset" + index + "spacing", "0");
            if (!mapProperties.ContainsKey("tileset" + index + "margin"))
                mapProperties.Add("tileset" + index + "margin", "0");
            ReadAttributes(tilesetElement.Descendants("image").First(), "tileset" + index + "image");
        }

        private void ReadLayer(XElement layerElement, int index)
        {
            ReadAttributes(layerElement, "layer" + index);
            if (!mapProperties.ContainsKey("layer" + index + "opacity"))
                mapProperties.Add("layer" + index + "opacity", "0");

            ReadProperties(layerElement, "layer" + index);
            ReadLayerData(layerElement, index);
        }

        private void ReadObjectGroup(XElement objectGroupElement, int objectGroupIndex)
        {
            ReadAttributes(objectGroupElement, "objectgroup" + objectGroupIndex);
            ReadProperties(GetPropertiesNode(objectGroupElement), "objectgroup" + objectGroupIndex);
            ReadObjectGroupObjects(objectGroupElement, objectGroupIndex);
        }

        private void ReadLayerData(XElement layerElement, int index)
        {
            ReadAttributes(layerElement.Descendants("data").First(), "layer" + index);
            if (!mapProperties.ContainsKey("layer" + index + "compression"))
                mapProperties.Add("layer" + index + "compression", "uncompressed");
            ReadDataString(layerElement, index);
        }

        private void ReadObjectGroupObjects(XElement objectGroupElement, int objectGroupIndex)
        {
            var objectQuery = from nodes in objectGroupElement.Descendants("object") select nodes;
            mapProperties.Add("objectgroup" + objectGroupIndex + "objectcount", objectQuery.Count() + "");

            for (int i = 0; i < objectQuery.Count(); i++)
            {
                string path = "objectgroup" + objectGroupIndex + "object" + i;
                ReadAttributes(objectQuery.ToList()[i], path);
                ReadProperties(objectQuery.ToList()[i], path);
            }
        }

        private void ReadDataString(XElement layerElement, int index)
        {
            var datastring = from nodes in layerElement.Descendants("data") select nodes;
            foreach (XElement s in datastring)
                mapProperties.Add("layer" + index + "data", s.Value);
        }

        private void ReadProperties(XElement element, string path)
        {
            var properties = from nodes in element.Descendants("property") select nodes;
            mapProperties.Add(path + "propertycount", properties.Count().ToString());

            for (int i = 0; i < properties.Count(); i++)
                ReadAttributes(properties.ToList()[i], path + "property" + i);
        }

        private void ReadAttributes(XElement element, string path)
        {
            var propertyAttributes = from nodes in element.Attributes() select nodes;
            foreach (XAttribute attribute in propertyAttributes)
                mapProperties.Add(path + attribute.Name.ToString(), attribute.Value.ToString());
        }

        private XElement GetPropertiesNode(XElement parent)
        {
            var element = parent.Descendants("properties");
            if (element.Count() != 0)
                return element.First();
            else
                return parent;
        }
    }
}
