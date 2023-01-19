using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PA.FileUtils
{
    public static class FileProvider
    {
        public static byte[] ToByteArray(string fileName)
        {
            byte[] result;
            FileStream fs = new FileStream(fileName, FileMode.Open);
            result = new byte[fs.Length];
            try
            {
                fs.Read(result, 0, result.Length);
            }
            finally
            {
                fs.Flush();
                fs.Close();
            }
            return result;
        }

        public static bool FromByteArray(string outputFile, byte[] buffer, bool overwrite)
        {
            bool result;
            if (File.Exists(outputFile) && !overwrite)
                return false;
            FileStream fs = new FileStream(outputFile, FileMode.Create);
            result = false;
            try
            {
                fs.Write(buffer, 0, buffer.Length);
            }
            finally
            {
                fs.Flush();
                fs.Close();
                result = true;
            }
            return result;
        }
    }
}
