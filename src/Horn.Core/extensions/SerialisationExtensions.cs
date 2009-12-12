using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Horn.Core.Extensions
{
    public static class SerialisationExtensions
    {
        public static T DescrialiseContractXml<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                ThrowArgumentNullException<T>();

            var buffer = Encoding.UTF8.GetBytes(xml);
            var memoryStream = new MemoryStream(buffer);
            var serializer = new DataContractSerializer(typeof(T));
            return (T)serializer.ReadObject(memoryStream);
        }

        public static string ToDataContractXml<T>(this object objectToSerialise)
        {
            var memoryStream = new MemoryStream();
            var writer = new XmlTextWriter(memoryStream, Encoding.UTF8);
            var serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(writer, objectToSerialise);
            writer.Flush();
            memoryStream = (MemoryStream)writer.BaseStream;
            memoryStream.Flush();
            return (new UTF8Encoding()).GetString(memoryStream.ToArray());
        }

        private static void ThrowArgumentNullException<T>()
        {
            throw new ArgumentNullException(
                string.Format("You must a pass a valid JSON string to SerializationHelper.Deserialise of type {0}", typeof(T)));
        }
    }
}