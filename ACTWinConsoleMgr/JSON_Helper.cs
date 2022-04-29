using ACT.Core.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace ACT.Applications.ConsoleManager
{
    public static class JSON_Helper
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

        internal static class FileProtectionHelper
        {
            public static string CleanupEncryptedFileLogic_ReturnEncryptedData(string UnEncryptedPath, string EncryptedPath, string BackupDirectory = "")
            {
                string _tmpReturn = null;
                bool FoundUnencryptedFile = false;
                bool FoundEncryptedFile = false;

                if (UnEncryptedPath.FileExists() == true) { FoundUnencryptedFile = true; }
                if (EncryptedPath.FileExists() == true) { FoundEncryptedFile = true; }

                // Replace Encrypted File After Archive
                if (FoundEncryptedFile == true && FoundUnencryptedFile == true)
                {
                    #region Create Backup -- Use Custom Path If Declared
                    // TODO Reduce Complexity
                    if (BackupDirectory.NullOrEmpty() == false)
                    {
                        if (BackupDirectory.EnsureDirectoryFormat().DirectoryExists() == false)
                        {
                            System.IO.File.Copy(UnEncryptedPath,
                            UnEncryptedPath.Replace(".json", "-" + DateTime.Now.ToUnixTime().ToString() + ".bak"));
                        }
                        else
                        {
                            string _Dest = BackupDirectory.EnsureDirectoryFormat() + UnEncryptedPath.GetFileNameFromFullPath()
                                .Replace(".json", "-" + DateTime.Now.ToUnixTime().ToString() + ".bak");

                            System.IO.File.Copy(UnEncryptedPath, _Dest);
                        }
                    }
                    else
                    {
                        System.IO.File.Copy(UnEncryptedPath,
                        UnEncryptedPath.Replace(".json", "-" + DateTime.Now.ToUnixTime().ToString() + ".bak"));
                    }
                    #endregion

                    System.Threading.Thread.Sleep(500);
                    System.IO.File.Delete(EncryptedPath);

                    try
                    {
                        _tmpReturn = ACT.Core.Security.ProtectData.ProtectStringToString(System.IO.File.ReadAllText(UnEncryptedPath), true);
                    }
                    catch // TODO LOG ERROR (Exception ex)
                    {
                        return null;
                    }

                    System.Threading.Thread.Sleep(500);
                    System.IO.File.Delete(UnEncryptedPath);

                    System.Threading.Thread.Sleep(500);
                    System.IO.File.WriteAllText(EncryptedPath, _tmpReturn);
                }
                else if (FoundEncryptedFile == false && FoundUnencryptedFile == true)
                {
                    try
                    {
                        _tmpReturn = ACT.Core.Security.ProtectData.ProtectStringToString(System.IO.File.ReadAllText(UnEncryptedPath), true);
                    }
                    catch // TODO LOG ERROR (Exception ex)
                    {
                        return null;
                    }
                    System.Threading.Thread.Sleep(500);
                    System.IO.File.Delete(UnEncryptedPath);

                    System.Threading.Thread.Sleep(500);
                    System.IO.File.WriteAllText(EncryptedPath, _tmpReturn);
                }
                else if (FoundEncryptedFile == false && FoundUnencryptedFile == false)
                {
                    // TODO LOG ERROR ErrorList_History.Add("Menu Not Found");
                    return null;
                }
                else
                {
                    try { _tmpReturn = System.IO.File.ReadAllText(EncryptedPath); }
                    catch // TODO LOG ERROR (Exception ex)
                    {
                        return null;
                    }
                }

                return _tmpReturn;
            }
        }

    }

    public static class Helper
    {
        public static string GetMultilineResponse(string Caption, string EndOfInputString, bool ShowEndOfInputStringCaption = true, bool AllowBlankLines = true, string StartMarkup = "", string EndMarkup = "", bool ReturnConsoleToOriginalMarkup = true)
        {
            string _tmpReturn = "";
            string _tmpLine = null;

            if (ReturnConsoleToOriginalMarkup) { }

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
