using System;
using System.Collections.Generic;

namespace ACT.Applications.ConsoleManager.Engine
{


    public static class Core
    {
        /// TODO Replace To ACT Extensions Method        
        public static ConsoleColor PSC(string ConsoleColorValue) { try { return (ConsoleColor)Enum.Parse(typeof(ConsoleColor), ConsoleColorValue); } catch { throw new Exception(ConsoleColorValue + " Is Not A Valid Console Color."); } }

        public static Structs.ACT_Menu Default_MenuObject = null;
        public static bool Initalized = false;
        public static string BaseDirectory { get; internal set; }
        public static string MenuBaseDirectory { get; internal set; }
        public static string DefaultDataBaseDirectory { get; internal set; }
        public static string ActiveMenuID;
        public static List<ACT_Menu_Instance> LoadedMenus = new List<ACT_Menu_Instance>();

        /// <summary>
        /// Initalize the Console Manager Engine
        /// </summary>
        internal static void Init()
        {
            BaseDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\Applications\\ConsoleManager\\";
            MenuBaseDirectory = BaseDirectory + "Menus\\";
            DefaultDataBaseDirectory = DefaultDataBaseDirectory + "Default\\";
            Initalized = true;


        }

        /// <summary>
        /// Value is seperated by equal sign
        /// Needs Value (bg,fg,txt,txtl) - No Value Needed (nl)
        /// Markup Value i,e, txt=Hello        
        /// </summary>
        /// <param name="Markup">Key/Value or Just Key</param>
        /// <param name="ConsolePointer"></param>
        public static void LoadMarkup(string MarkupID, string Markup, bool ContinueOnError = true)
        {
            string[] _MarkupData = Markup.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            uint _Line = 0;
            bool InCSharpSection = true;
            bool InActionSection = true;

            string _CSharpCode = "";
            string _ActionCode = "";
            uint _LineStart = 0;
            Dictionary<uint, string> _AllProperties = new Dictionary<uint, string>();
            Dictionary<uint, string> _AllMethods = new Dictionary<uint, string>();
            Dictionary<uint, string> _AllCSharpCodes = new Dictionary<uint, string>();
            Dictionary<uint, string> _AllActions = new Dictionary<uint, string>();

            foreach (var _Markup in _MarkupData)
            {
                _Line++;
                string _Value = "";

                // Gather All CSharp Code
                if (InCSharpSection)
                {
                    if (IsEndOfCSharpSection(_Markup))
                    {
                        _AllCSharpCodes.Add(_LineStart, _Markup);
                        InCSharpSection = false;
                        continue;
                    }
                    _CSharpCode += _Markup;
                    continue;
                }

                // Gather All Action Code
                if (InActionSection)
                {
                    if (IsEndOfActionSection(_Markup))
                    {
                        _AllCSharpCodes.Add(_LineStart, _Markup);
                        InActionSection = false;
                        continue;
                    }
                    _ActionCode += _Markup;
                    continue;
                }

                // Check For Property
                if (IsProperty(_Markup))
                {
                    try { _Value = _Markup.Trim().Substring(_Markup.IndexOf('=') + 1); }
                    catch (Exception Ex)
                    {
                        System.Diagnostics.Trace.WriteLine("Error Processing: " + _Markup);
                        if (ContinueOnError) { continue; }
                        throw new Exception("Error Processing Line: " + _Line.ToString() + " : (" + Ex.Message + ")");
                    }
                    _AllProperties.Add(_Line, _Markup);
                    continue;
                }

                // Check For Method
                if (IsMethodCall(_Markup))
                {
                    try { _Value = _Markup.Trim().Substring(_Markup.IndexOf('(') + 1, _Markup.IndexOf(')')); }
                    catch (Exception Ex)
                    {
                        System.Diagnostics.Trace.WriteLine("Error Processing: " + _Markup);
                        if (ContinueOnError) { continue; }
                        throw new Exception("Error Processing Line: " + _Line.ToString() + " : (" + Ex.Message + ")");
                    }

                    _AllMethods.Add(_Line, _Markup);
                    continue;
                }

                // Check For CSharp Code
                if (IsCSharpSectionStart(_Markup))
                {
                    _LineStart = _Line;
                    InCSharpSection = false;
                    continue;
                }

                // Check For Action Definition
                if (IsActionStart(_Markup))
                {
                    _LineStart = _Line;
                    InActionSection = true;
                    continue;
                }

                // Check For Action Definition
                if (IsEndOfActionSection(_Markup))
                {
                    _LineStart = _Line;
                    InActionSection = false;
                    continue;
                }

            }

            Structs.ACT_MarkupFile _NewCode = new Structs.ACT_MarkupFile(MarkupID);
            _NewCode.SetData(_AllProperties, _AllMethods, _AllCSharpCodes, _AllActions);
        }

        private static bool IsProperty(string Value)
        {
            Value = Value.Trim();
            if (Value.Contains("="))
            {
                if (Value.IndexOf('=') < 2) { return false; }
            }
            return true;
        }
        private static bool IsMethodCall(string Value)
        {
            Value = Value.Trim();
            if (Value.IndexOf('(') < 3) { return false; }
            if (Value.Contains("(") && Value.Contains(")")) { if (Value.IndexOf('(') < Value.IndexOf(')')) { return true; } }
            return false;
        }
        private static bool IsCSharpSectionStart(string Value)
        {
            Value = Value.Trim();
            if (Value.StartsWith("###CS###")) { return true; }
            return false;
        }
        private static bool IsEndOfCSharpSection(string Value)
        {
            Value = Value.Trim();
            if (Value.StartsWith("###ENDCS###")) { return true; }
            return false;
        }
        private static bool IsActionStart(string Value)
        {
            Value = Value.Trim();
            if (Value.StartsWith("action=")) { return true; }
            return false;
        }
        private static bool IsEndOfActionSection(string Value)
        {
            Value = Value.Trim();
            if (Value.StartsWith("endaction=")) { return true; }
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
