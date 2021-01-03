using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SharpCompress.Compressor.BZip2;


namespace xx.Helper.Utils
{
    public class DecompressData
    {
        public static string DecompressString(byte[] byteArray)
        {
            MemoryStream ms = new MemoryStream(byteArray);
            BZip2Stream gzip = new BZip2Stream(ms, SharpCompress.Compressor.CompressionMode.Decompress, true);

            byte[] buffer = StreamToByteArray(gzip);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < buffer.Length; i++)
            {
                sb.Append((char)buffer[i]);
            }
            gzip.Flush();
            gzip.Dispose();
            ms.Dispose();
            return sb.ToString();
        }

        public static byte[] StreamToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            MemoryStream ms = new MemoryStream();
            int read;

            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }
            return ms.ToArray();
        }
    }
}
