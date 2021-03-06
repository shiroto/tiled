using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;


namespace Tiled.TMXProcessorLib
{
    [ContentImporter(".tmx", DefaultProcessor = "TMXProcessor",
          DisplayName = "TMX Importer")]
    internal class TMXImporter : ContentImporter<XDocument>
    {
        public override XDocument Import(string filename,
            ContentImporterContext context)
        {
            string filecontent = File.ReadAllText(filename);
            return XDocument.Load(new StringReader(filecontent));
        }
    }
}
