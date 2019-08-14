using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Opw.HttpExceptions
{
    public static class SerializationHelper
    {
        public static T SerializeDeserialize<T>(T obj)
        {
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, obj);

            stream.Position = 0;
            return (T)formatter.Deserialize(stream);
        }
    }
}
