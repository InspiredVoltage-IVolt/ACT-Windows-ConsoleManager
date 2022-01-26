using ACT.Core.Extensions;
using System;

namespace ACT.Applications.ConsoleManager.ConsoleMarkupManager
{
    public static class Core
    {
        static ConsoleColor PSC(string x) { try { return (ConsoleColor)Enum.Parse(typeof(ConsoleColor), x); } catch { throw new Exception(x + " Is Not A Valid Console Color."); } }

        /// <summary>
        /// Value is seperated by equal sign
        /// Needs Value (bg,fg,txt,txtl) - No Value Needed (nl)
        /// Markup Value i,e, txt=Hello        
        /// </summary>
        /// <param name="Markup">Key/Value or Just Key</param>
        /// <param name="ConsolePointer"></param>
        public static void ProcessMarkup(string Markup)
        {
            string[] _MarkupData = Markup.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (var _Markup in _MarkupData)
            {
                string _Value = "";

                if (_Value.ToLower() != "nl")
                {
                    try { _Value = _Markup.Trim().Substring(_Markup.IndexOf('=') + 1); } catch { System.Diagnostics.Trace.WriteLine("Error Processing: " + _Markup); continue; }
                }

                if (_Markup.ToLower().StartsWith("bg"))
                {
                    Console.BackgroundColor = PSC(_Value);
                }
                else if (_Markup.ToLower().StartsWith("fg"))
                {
                    Console.ForegroundColor = PSC(_Value);
                }
#if WINDOWS
                else if (_Markup.ToLower().StartsWith("cursorsize"))
                {
#pragma warning disable CA1416 // Validate platform compatibility
                    try { Console.CursorSize = _Value.ToIntFast(); } catch { }
#pragma warning restore CA1416 // Validate platform compatibility
                }
#endif
                else if (_Markup.ToLower().StartsWith("cursorvisible"))
                {
                    Console.CursorVisible = _Value.ToBool(true);
                }
                else if (_Markup.ToLower().StartsWith("txt"))
                {
                    Console.Write(_Value);
                }
                else if (_Markup.ToLower().StartsWith("txtl"))
                {
                    Console.WriteLine(_Value);
                }
                else if (_Markup.ToLower().StartsWith("nl"))
                {
                    Console.WriteLine("");
                }
            }
        }

        /// <summary>
        /// Generate Current Markup and Save as MarkupText
        /// </summary>
        /// <returns>ACT ConsoleMarkup</returns>
        public static string GenerateCurrentMarkup()
        {
            string _tmpReturn = "";

            _tmpReturn += "bg=" + Console.BackgroundColor.ToString() + Environment.NewLine;
            _tmpReturn += "fg=" + Console.ForegroundColor.ToString() + Environment.NewLine;
            _tmpReturn += "cursorsize=" + Console.CursorSize.ToString() + Environment.NewLine;
#if WINDOWS
#pragma warning disable CA1416 // Validate platform compatibility
            _tmpReturn += "cursorvisible=" + Console.CursorVisible.ToString() + Environment.NewLine;
#pragma warning restore CA1416 // Validate platform compatibility
#endif

            return _tmpReturn;
        }
    }
}
