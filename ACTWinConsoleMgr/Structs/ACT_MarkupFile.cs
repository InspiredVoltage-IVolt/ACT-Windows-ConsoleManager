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

        public ACT_MarkupFile(string ID) { MarkupCode_ID = ID; }

        // PRIVATE VARIABLES
        Dictionary<uint, string> AllProperties = new Dictionary<uint, string>();
        Dictionary<uint, string> AllMethods = new Dictionary<uint, string>();
        Dictionary<uint, string> AllCSharpCodes = new Dictionary<uint, string>();
        Dictionary<uint, string> AllActions = new Dictionary<uint, string>();
        SortedDictionary<uint, Enums.MarkupCodeType> _ExecutionData = new SortedDictionary<uint, Enums.MarkupCodeType>();
        byte[] _CompiledData = null;

        // PUBLIC VARIABLES
        /// <summary>
        /// ACT MarkupCode File ID (Usually MenuName_FileName) Can Be Anything Unique
        /// </summary>
        public string MarkupCode_ID { get; set; }
        public DateTime LastProcessedDateTime = DateTime.Now;
        public DateTime LastCompileDateTime = DateTime.Now;
        public DateTime LastExecuteDateTime = DateTime.Now;
        public bool CompiledSuccessfull = false;

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

        void PreCompile()
        {
            foreach (var i in AllProperties) { _ExecutionData.Add(i.Key, Enums.MarkupCodeType.Properties); }
            foreach (var i in AllMethods) { _ExecutionData.Add(i.Key, Enums.MarkupCodeType.Methods); }
            foreach (var i in AllCSharpCodes) { _ExecutionData.Add(i.Key, Enums.MarkupCodeType.CSharpCode); }
            foreach (var i in AllActions) { _ExecutionData.Add(i.Key, Enums.MarkupCodeType.Actions); }
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

            _CompiledData = _tmpReturn.ToArray();

            if (_CompiledData.Length % 8 == 0) { CompiledSuccessfull = true; }
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
            while (Position < _CompiledData.Length)
            {
                _CommandToExecute = _CompiledData[Position];

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

        private void ProcessProperty(uint? IndexPos) { }
        private void ProcessMethod(uint? IndexPos) { }
        private void ProcessAction(uint? IndexPos) { }
        private void ProcessCSCode(uint? IndexPos) { }
    }
}
