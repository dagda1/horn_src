using System;
using System.IO;

namespace Horn.Framework.helpers
{
    public static class FileHelper
    {

        public static void CreateFileWithRandomData(string path)
        {
            var dataArray = new byte[100000];
            new Random().NextBytes(dataArray);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                for (int i = 0; i < dataArray.Length; i++)
                {
                    fileStream.WriteByte(dataArray[i]);
                }

                fileStream.Seek(0, SeekOrigin.Begin);

                for (int i = 0; i < fileStream.Length; i++)
                {
                    if (dataArray[i] != fileStream.ReadByte())
                    {
                        Console.WriteLine("Error writing data.");
                        return;
                    }
                }
            }            
        }



    }
}