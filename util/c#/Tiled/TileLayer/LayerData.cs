using System;
using System.IO;
using System.IO.Compression;
using Microsoft.Xna.Framework.Graphics;

using Tiled.Exceptions;

namespace Tiled.TileLayer
{
    internal struct LayerData
    {
        private string encoding;
        private string compression;
        public int[,] globalTileID;
        public SpriteEffects[,] tileRotation;
        public string dataString;

        public LayerData(string dataString, string encoding, string compression, Layer layer)
        {
            this.dataString = dataString;
            this.encoding = encoding;
            this.compression = compression;
            globalTileID = new int[layer.Width, layer.Height];
            tileRotation = new SpriteEffects[layer.Width, layer.Height];
            ReadAndDecodeDataString(layer);
        }

        private void ReadAndDecodeDataString(Layer layer)
        {
            switch (encoding)
            {
                case "base64":
                    char[] encodedString = dataString.Trim().ToCharArray();
                    byte[] decodedString = Convert.FromBase64CharArray(encodedString, 0, encodedString.Length);
                    DecompressAndWriteData(GetStream(decodedString, layer));
                    break;
                default:
                    throw new UnsupportedEncodingException("Layer \"" + layer + "\" has unsupported encoding.");
            }
        }

        private void DecompressAndWriteData(Stream stream)
        {
            const int size = 4;
            byte[] buffer = new byte[size];
            int x = 0, y = 0;
            using (MemoryStream memory = new MemoryStream())
            {
                int count = 0;
                do
                {
                    count = stream.Read(buffer, 0, size);
                    if (count > 0)
                    {
                        memory.Write(buffer, 0, count);

                        WriteGlobalTileID(buffer, x, y);
                        WriteRotation(buffer, x, y);
                    }
                    x++;
                    if (x == globalTileID.GetLength(0))
                    {
                        x = 0;
                        y++;
                    }
                }
                while (count > 0);
            }
        }

        private Stream GetStream(byte[] data, Layer layer)
        {
            Stream stream;
            switch (compression)
            {
                case "gzip":
                    stream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress);
                    break;
                case "uncompressed":
                    stream = new MemoryStream(data);
                    break;
                case "zlib":
                    stream = new Ionic.Zlib.ZlibStream(new MemoryStream(data), Ionic.Zlib.CompressionMode.Decompress);                       
                    break;
                default:
                    throw new UnsupportedCompressionException("Layer \"" + layer + "\" has unsupported compression.");
            }
            return stream;
        }

        private void WriteGlobalTileID(byte[] buffer, int x, int y)
        {
            int globalTileId = buffer[0];
            globalTileId |= buffer[1] << 8;
            globalTileId |= buffer[2] << 8;
            globalTileID[x, y] = globalTileId - 1;
        }

        private void WriteRotation(byte[] buffer, int x, int y)
        {
            switch (buffer[3])
            {
                case 64:
                    tileRotation[x, y] = SpriteEffects.FlipVertically;
                    break;
                case 128:
                    tileRotation[x, y] = SpriteEffects.FlipHorizontally;
                    break;
                case 192:
                    tileRotation[x, y] = new SpriteEffects();
                    tileRotation[x, y] += 1;
                    tileRotation[x, y] += 2;
                    break;
            }
        }
    }
}
