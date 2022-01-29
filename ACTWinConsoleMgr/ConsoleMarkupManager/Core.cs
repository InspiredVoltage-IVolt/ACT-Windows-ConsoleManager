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
        public static void ProcessMarkup(string Markup, bool ContinueOnError = true)
        {
            string[] _MarkupData = Markup.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            int _Line = 0;
            foreach (var _Markup in _MarkupData)
            {
                _Line++;

                #region Process Properties And Simple Actions
                bool _PropertyFound = false;
                string _Value = "";
                // Check For Property
                if ()
                {
                    try { _Value = _Markup.Trim().Substring(_Markup.IndexOf('=') + 1); }
                    catch (Exception Ex)
                    {
                        System.Diagnostics.Trace.WriteLine("Error Processing: " + _Markup);
                        if (ContinueOnError) { continue; }
                        throw new Exception("Error Processing Line: " + _Line.ToString() + " : (" + Ex.Message + ")");
                    }
                    _PropertyFound = true;
                }
                // Check For Method
                if (IsMethod(_Markup))
                {
                    try { _Value = _Markup.Trim().Substring(_Markup.IndexOf('(') + 1, _Markup.IndexOf(')')); }
                    catch (Exception Ex)
                    {
                        System.Diagnostics.Trace.WriteLine("Error Processing: " + _Markup);
                        if (ContinueOnError) { continue; }
                        throw new Exception("Error Processing Line: " + _Line.ToString() + " : (" + Ex.Message + ")");
                    }
                    _PropertyFound = true;
                }
                // Check For CSharp Code


                if (_PropertyFound)
                {
                    if (_Markup.ToLower().StartsWith("bg"))
                    {
                        Console.BackgroundColor = PSC(_Value);
                    }
                    else if (_Markup.ToLower().StartsWith("fg"))
                    {
                        Console.ForegroundColor = PSC(_Value);
                    }
                    else if (_Markup.ToLower().StartsWith("cursorvisible") || _Markup.ToLower().StartsWith("hidecursor"))
                    {
                        Console.CursorVisible = _Value.ToBool(true);
                    }
                    else if (_Markup.ToLower().StartsWith("txt") || _Markup.ToLower().StartsWith("wr"))
                    {
                        Console.Write(_Value);
                    }
                    else if (_Markup.ToLower().StartsWith("txtl") || _Markup.ToLower().StartsWith("wrl"))
                    {
                        Console.WriteLine(_Value);
                    }
                    else if (_Markup.ToLower().StartsWith("nl") || _Markup.ToLower().StartsWith("crlf"))
                    {
                        Console.WriteLine("");
                    }
                }
            }
        }

        private static bool IsProperty(string Value)
        {
            Value = Value.Trim();
            if (Value.Contains("="))
            {
                if (Value.IndexOf())
                { }
            }
        }

        private static bool IsMethod(string Value)
        {
            Value = Value.Trim();
            if (Value.IndexOf('(') < 3) { return false; }
            if (Value.Contains("(") && Value.Contains(")")) { if (Value.IndexOf('(') < Value.IndexOf(')')) { return true; } }
            return false;
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
