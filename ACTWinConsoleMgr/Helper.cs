using ACT.Core.Extensions;
using System;

namespace ACT.Applications.ConsoleManager
{
    public static class Helper
    {

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
