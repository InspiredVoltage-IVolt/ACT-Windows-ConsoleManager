using ACT.Core.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace ACT.Applications.ConsoleManager
{
    public static class Helper
    {
        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            };
        }

        internal class ParseStringConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                {
                    return null;
                }

                var value = serializer.Deserialize<string>(reader);
                long l;
                if (Int64.TryParse(value, out l))
                {
                    return l;
                }
                throw new Exception("Cannot unmarshal type long");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (long)untypedValue;
                serializer.Serialize(writer, value.ToString());
                return;
            }

            public static readonly ParseStringConverter Singleton = new ParseStringConverter();
        }

        public static string GetMultilineResponse(string Caption, string EndOfInputString, bool ShowEndOfInputStringCaption = true, bool AllowBlankLines = true, string StartMarkup = "", string EndMarkup = "", bool ReturnConsoleToOriginalMarkup = true)
        {
            string _tmpReturn = "";
            string _tmpLine = null;

            if (ReturnConsoleToOriginalMarkup) { }
            if (StartMarkup.NullOrEmpty() == false) { ConsoleMarkupManager.Core.ProcessMarkup(StartMarkup); }



            while (_tmpLine != EndOfInputString)
            {
                _tmpLine = Console.ReadLine() ?? "";
                if (_tmpLine == EndOfInputString) { continue; }
                else { _tmpReturn += _tmpLine; }
            }

            return _tmpReturn;
        }

    }
}
