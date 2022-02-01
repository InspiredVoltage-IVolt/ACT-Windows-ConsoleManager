using ACT.Core.Extensions;
using System;
using System.Collections.Generic;

namespace ACT.Applications.ConsoleManager.Structs
{
    /// <summary>
    /// Class Containing Processed Markup Code
    /// </summary>
    public class ACT_MarkupFile
    {
        // Constructor
        public ACT_MarkupFile() { }

        // Constructor
        public ACT_MarkupFile(string MenuName, string MarkupFile, bool IgnoreErrors = false)
        {
            if (MarkupFile.FileExists() == false) // TODO LOG ERROR
            {
                throw new Exception("Error: Markup File Does Not Exist: '" + MarkupFile + "'");
            }

            MarkupFileFullPath = MarkupFile;
            MarkupFile_ID = MenuName.Replace("|", "###BAR###") + "|" + MarkupFile.GetFileNameWithoutExtension().Replace("|", "###BAR###");

            // TODO IMPLEMENT PROTECTION
            //try { }
            //catch { }

            LoadMarkup(MarkupFileFullPath.ReadAllText(), IgnoreErrors);
        }

        #region PRIVATE VARIABLES
        string MarkupFileFullPath = "";
        Dictionary<uint, string> AllProperties = new Dictionary<uint, string>();
        Dictionary<uint, string> AllMethods = new Dictionary<uint, string>();
        Dictionary<uint, string> AllCSharpCodes = new Dictionary<uint, string>();
        Dictionary<uint, string> AllActions = new Dictionary<uint, string>();
        SortedDictionary<uint, Enums.MarkupCodeType> _ExecutionData = new SortedDictionary<uint, Enums.MarkupCodeType>();

        #endregion

        #region PUBLIC VARIABLES
        /// <summary>
        /// ACT MarkupCode File ID (Usually MenuName_FileName) Can Be Anything Unique
        /// </summary>
        public string MarkupFile_ID { get; set; }

        /// <summary>
        /// Grab Menu Name From MarkupFileID
        /// </summary>
        public string MenuName
        {
            get
            {
                try
                {
                    return MarkupFile_ID.SplitString("|", StringSplitOptions.RemoveEmptyEntries)[0];// TODO LOG ERROR
                }
                catch { return null; }
            }
        }

        /// <summary>Markup Contents</summary>
        public string MarkupContents { get; set; }
        public DateTime LastProcessedDateTime = DateTime.Now;
        public DateTime LastCompileDateTime = DateTime.Now;
        public DateTime LastExecuteDateTime = DateTime.Now;
        public bool CompiledSuccessfull = false;
        public byte[] CompiledMarkup_Data = null;
        #endregion

        #region Private Methods

        private bool IsProperty(string Value)
        {
            Value = Value.Trim();
            if (Value.Contains("="))
            {
                if (Value.IndexOf('=') < 2) { return false; }
            }
            return true;
        }
        private bool IsMethodCall(string Value)
        {
            Value = Value.Trim();
            if (Value.IndexOf('(') < 3) { return false; }
            if (Value.Contains("(") && Value.Contains(")")) { if (Value.IndexOf('(') < Value.IndexOf(')')) { return true; } }
            return false;
        }
        private bool IsCSharpSectionStart(string Value)
        {
            Value = Value.Trim();
            if (Value.StartsWith("###CS###")) { return true; }
            return false;
        }
        private bool IsEndOfCSharpSection(string Value)
        {
            Value = Value.Trim();
            if (Value.StartsWith("###ENDCS###")) { return true; }
            return false;
        }
        private bool IsActionStart(string Value)
        {
            Value = Value.Trim();
            if (Value.StartsWith("action=")) { return true; }
            return false;
        }
        private bool IsEndOfActionSection(string Value)
        {
            Value = Value.Trim();
            if (Value.StartsWith("endaction=")) { return true; }
            return false;
        }

        private void PreCompile()
        {
            foreach (var i in AllProperties) { _ExecutionData.Add(i.Key, Enums.MarkupCodeType.Properties); }
            foreach (var i in AllMethods) { _ExecutionData.Add(i.Key, Enums.MarkupCodeType.Methods); }
            foreach (var i in AllCSharpCodes) { _ExecutionData.Add(i.Key, Enums.MarkupCodeType.CSharpCode); }
            foreach (var i in AllActions) { _ExecutionData.Add(i.Key, Enums.MarkupCodeType.Actions); }
        }
        private void ProcessProperty(uint? IndexPos) { }
        private void ProcessMethod(uint? IndexPos) { }
        private void ProcessAction(uint? IndexPos) { }
        private void ProcessCSCode(uint? IndexPos) { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Set The Processed Data
        /// </summary>
        /// <param name="AllProperties">Parsed Properties</param>
        /// <param name="AllMethods">Parsed Methods</param>
        /// <param name="AllCSharpCodes">Parsed CSharpCodes</param>
        /// <param name="AllActions">Parsed Actions</param>
        public void SetData(Dictionary<uint, string> LoadedProperties, Dictionary<uint, string> LoadedMethods,
            Dictionary<uint, string> LoadedCSharpCodes, Dictionary<uint, string> LoadedActions)
        {
            AllProperties = LoadedProperties;
            AllMethods = LoadedMethods;
            AllCSharpCodes = LoadedCSharpCodes;
            AllActions = LoadedActions;
            LastProcessedDateTime = DateTime.Now;
        }

        /// <summary>
        /// Value is seperated by equal sign
        /// Needs Value (bg,fg,txt,txtl) - No Value Needed (nl)
        /// Markup Value i,e, txt=Hello        
        /// </summary>
        /// <param name="Markup">Key/Value or Just Key</param>
        /// <param name="ConsolePointer"></param>
        public void LoadMarkup(string Markup, bool ContinueOnError = true)
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

            //Structs.ACT_MarkupFile _NewCode = new Structs.ACT_MarkupFile(MarkupID);
            SetData(_AllProperties, _AllMethods, _AllCSharpCodes, _AllActions);
        }

        /// <summary>
        /// Compile the Code Into Executable Byte Order
        /// </summary>
        /// <returns></returns>
        public bool Compile()
        {
            PreCompile();

            List<byte> _tmpReturn = new List<byte>();
            ulong _DataItem = 0;

            foreach (var i in _ExecutionData.Keys)
            {
                _DataItem = 0;
                _DataItem = _DataItem + i;
                _DataItem = _DataItem << 32;
                _DataItem = _DataItem | (ulong)_ExecutionData[i];
                _tmpReturn.AddRange(_DataItem.ObjectToByteArray());
            }

            CompiledMarkup_Data = _tmpReturn.ToArray();

            if (CompiledMarkup_Data.Length % 8 == 0) { CompiledSuccessfull = true; }
            else { CompiledSuccessfull = false; }

            return CompiledSuccessfull;
        }

        /// <summary>
        /// Execute the Processing Data Compiled Code
        /// </summary>
        /// <param name="ReCompile"></param>
        /// <exception cref="Exception"></exception>
        public void Execute(bool ReCompile = false)
        {
            if (ReCompile) { Compile(); }
            if (CompiledSuccessfull == false) { throw new Exception("No Valid Compiled Code Found"); }

            var Position = 0;
            uint? _IndexPosition = null;
            uint? _MarkupCodeType = null;
            ulong? _CommandToExecute = null;
            while (Position < CompiledMarkup_Data.Length)
            {
                _CommandToExecute = CompiledMarkup_Data[Position];

                var _tmpBytes = _CommandToExecute.ObjectToByteArray();

                _MarkupCodeType = (uint)_CommandToExecute >> 32;
                _IndexPosition = (uint)_CommandToExecute & uint.MaxValue;

                if (_MarkupCodeType == (uint)Enums.MarkupCodeType.Properties) { ProcessProperty(_IndexPosition); }
                else if (_MarkupCodeType == (uint)Enums.MarkupCodeType.Methods) { ProcessMethod(_IndexPosition); }
                else if (_MarkupCodeType == (uint)Enums.MarkupCodeType.Actions) { ProcessAction(_IndexPosition); }
                else if (_MarkupCodeType == (uint)Enums.MarkupCodeType.CSharpCode) { ProcessCSCode(_IndexPosition); }
                else { throw new Exception("Unkown Processor Command At Index: " + Position); }
            }
        }

        #endregion

    }
}
