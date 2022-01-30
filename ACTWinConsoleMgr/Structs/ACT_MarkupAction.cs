using System.Collections.Generic;

namespace ACT.Applications.ConsoleManager.Structs
{
    /// <summary>
    /// Action Definition
    /// </summary>
    public class ACT_MarkupAction
    {
        public string ActionName;
        public string ActionMarkup;
        public List<string> ActionProperties = new List<string>();
    }
}
