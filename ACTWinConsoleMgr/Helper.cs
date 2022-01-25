using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACT.Applications.WinConsoleMgr
{
    public static class Helper
    {

        public static string GetMultilineResponse(string EndOfInputString, bool AllowBlankLines = true)
        {
            string _tmpReturn = "";
            string? _tmpLine = null;

            while(_tmpLine!= EndOfInputString)
            {
                _tmpLine = Console.ReadLine()??"";
                if (_tmpLine == EndOfInputString) { continue; }
                else { _tmpReturn += _tmpLine; }
            }

            return _tmpReturn;
        }

    }
}
