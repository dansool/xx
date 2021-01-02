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
        //public static string DecompressString(byte[] data)
        //{
        //    var x = UnzipExtensions.UnzipString(data);
        //    return x;
        //}

        public static string DecompressString(byte[] byteArray)
        {
            //Prepare for decompress
            MemoryStream ms = new MemoryStream(byteArray);
            BZip2Stream gzip = new BZip2Stream(ms, SharpCompress.Compressor.CompressionMode.Decompress, true);

            //Decompress
            byte[] buffer = StreamToByteArray(gzip);

            //Transform byte[] unzip data to string
            StringBuilder sb = new StringBuilder();

            //Read the number of bytes GZipStream red and do not a for each bytes in resultByteArray;
            for (int i = 0; i < buffer.Length; i++)
                sb.Append((char)buffer[i]);

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
                ms.Write(buffer, 0, read);

            return ms.ToArray();
        }
    }
}
