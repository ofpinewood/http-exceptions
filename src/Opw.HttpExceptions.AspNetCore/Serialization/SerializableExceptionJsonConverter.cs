using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Opw.HttpExceptions.AspNetCore.Serialization
{
    internal class SerializableExceptionJsonConverter : JsonConverter<SerializableException>
    {
        private static readonly JsonEncodedText Type = JsonEncodedText.Encode("type");
        private static readonly JsonEncodedText HelpLink = JsonEncodedText.Encode("helpLink");
        private static readonly JsonEncodedText HResult = JsonEncodedText.Encode("hResult");
        private static readonly JsonEncodedText Message = JsonEncodedText.Encode("message");
        private static readonly JsonEncodedText Source = JsonEncodedText.Encode("source");
        private static readonly JsonEncodedText StackTrace = JsonEncodedText.Encode("stackTrace");
        private static readonly JsonEncodedText Data = JsonEncodedText.Encode("data");
        private static readonly JsonEncodedText InnerException = JsonEncodedText.Encode("innerException");

        public override SerializableException Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var serializableException = new SerializableException();

            if (!reader.Read())
            {
                throw new JsonException("UnexpectedJsonEnd");
            }

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                ReadValue(ref reader, serializableException);
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException("UnexpectedJsonEnd");
            }

            return serializableException;
        }

        public override void Write(Utf8JsonWriter writer, SerializableException value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        internal static void ReadValue(ref Utf8JsonReader reader, SerializableException value)
        {
            if (TryReadStringProperty(ref reader, Type, out var propertyValue))
            {
                value.Type = propertyValue;
            }
            else if (TryReadStringProperty(ref reader, HelpLink, out propertyValue))
            {
                value.HelpLink = propertyValue;
            }
            else if (TryReadStringProperty(ref reader, Message, out propertyValue))
            {
                value.Message = propertyValue;
            }
            else if (TryReadStringProperty(ref reader, Source, out propertyValue))
            {
                value.Source = propertyValue;
            }
            else if (TryReadStringProperty(ref reader, StackTrace, out propertyValue))
            {
                value.StackTrace = propertyValue;
            }
            else if (reader.TokenType == JsonTokenType.PropertyName && reader.ValueTextEquals(HResult.EncodedUtf8Bytes))
            {
                reader.Read();
                if (reader.TokenType == JsonTokenType.Null)
                {
                    // Nothing to do here.
                }
                else
                {
                    value.HResult = reader.GetInt32();
                }
            }
            else
            {
                var propertyName = reader.GetString();

                string json;
                using (var document = JsonDocument.ParseValue(ref reader))
                {
                    json = document.RootElement.Clone().GetRawText();
                }

                if (propertyName == Data.ToString())
                {
                    // TODO: Data property has not been implemented
                }
                else if (propertyName == InnerException.ToString())
                {
                    try
                    {
                        value.InnerException = json.ReadAsSerializableException();
                    }
                    catch { }
                }
            }
        }

        internal static bool TryReadStringProperty(ref Utf8JsonReader reader, JsonEncodedText propertyName, out string value)
        {
            if (reader.TokenType != JsonTokenType.PropertyName || !reader.ValueTextEquals(propertyName.EncodedUtf8Bytes))
            {
                value = default;
                return false;
            }

            reader.Read();
            value = reader.GetString();
            return true;
        }
    }
}