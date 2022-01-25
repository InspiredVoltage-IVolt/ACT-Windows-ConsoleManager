using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACT.Applications.WinConsoleMgr.ConsoleMarkupManager
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

            foreach(var _Markup in _MarkupData)
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
    }
}
