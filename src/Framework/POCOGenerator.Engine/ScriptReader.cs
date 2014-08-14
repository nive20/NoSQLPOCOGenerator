/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using POCOGenerator.Cassandra;
using POCOGenerator.DatabaseObjects;  
using POCOGenerator.LanguageProvider;

[assembly: CLSCompliant(true)]
namespace POCOGenerator.Engine
{
    public static class ScriptReader
    {
        #region Global Variables

        static string _keyspaceName = string.Empty;
        static string _columnFamilyName = string.Empty;
        static string _keyDataType = string.Empty;
        static string _columnDetails = string.Empty;
        static List<PocoObjectListForExport> _exportPocoList = new List<PocoObjectListForExport>();

        #endregion

        #region List generation from DBScript (CQL/CLI) to POCO

        /// <summary>
        /// This function Generates List from the CLI/ CQL Script Provided.
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <param name="script"></param>
        /// <param name="errorMessage"></param>
        /// <param name="isCqlScript"></param>
        /// <returns>Generated List<POCOObjectListForExport/></returns>
        public static List<PocoObjectListForExport> GenerateListFromScriptDbToPoco(string namespaceName, string script, ref string errorMessage, ref bool isCqlScript)
        {

            var columnsAndDataType = new Dictionary<string, string>();
            const string cliScript = "create column family";
            const string cqlScript = "create table";

            if (script.Contains(cliScript))
                GenerateListForScriptCli(namespaceName, script, ref errorMessage, isCqlScript, ref columnsAndDataType);

            else if (script.Contains(cqlScript))
                GenerateListForScriptCql(namespaceName, ref script, ref errorMessage, ref isCqlScript, ref columnsAndDataType);

            else errorMessage = "Enter only CQL/CLI Script";
            return _exportPocoList;
        }

        /// <summary>
        /// This function Generates List from the CQL Script Provided decided after it is checked in previuos Function.
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <param name="script"></param>
        /// <param name="errorMessage"></param>
        /// <param name="isCqlScript"></param>
        /// <param name="columnsAndDataType"></param>
        /// <returns>Generates List<POCOObjectListForExport/> Retained As Global Variable</returns>
        private static void GenerateListForScriptCql(string namespaceName, ref string script, ref string errorMessage, ref bool isCqlScript, ref Dictionary<string, string> columnsAndDataType)
        {
            if (columnsAndDataType == null) throw new ArgumentNullException("columnsAndDataType");

            var fetchdatafromScript = new FetchDataFromScript();

            if (script.Contains("with"))
                script = script.Remove(script.IndexOf("with", StringComparison.Ordinal));
            isCqlScript = true;
            columnsAndDataType = new Dictionary<string, string>();
            string createColumnFamilyCql = "create table if not exists";
            const string createColumnFamilyTableCql = "create table";

            bool isValidColumnFamily = Validator.ValidateCqlScript(script, ref errorMessage, ref createColumnFamilyCql, createColumnFamilyTableCql);

            if (isValidColumnFamily)
            {
                // Fetch Column family(Table) name
                _columnFamilyName = fetchdatafromScript.FetchData(script, createColumnFamilyCql, "(");
                _columnFamilyName = _columnFamilyName.Substring(_columnFamilyName.IndexOf(".", StringComparison.Ordinal) + 1);
                _columnFamilyName = _columnFamilyName.Trim('(', ')');
                // Fetch Column family(Table) Details
                _columnDetails = fetchdatafromScript.FetchData(script, "(", ")");

                if (_columnDetails.Contains("primary key"))
                {
                    var index = _columnDetails.IndexOf("primary key", StringComparison.Ordinal);
                    _columnDetails = _columnDetails.Remove(index, 11);
                    var getindex = _columnDetails.IndexOfAny(new[] { '(' }, index);
                    if (getindex > 0)
                    {
                        if (_columnDetails.ElementAt(getindex).Equals('('))
                        {
                            var lastindex = _columnDetails.LastIndexOf('(');
                            _columnDetails = _columnDetails.Remove(lastindex);
                        }
                    }
                }
                _columnDetails = _columnDetails.Trim(')');
                isValidColumnFamily = Validator.ValidateCqlScriptsColumnDetails(ref errorMessage, _columnFamilyName, _columnDetails);

                if (!isValidColumnFamily) return;
                columnsAndDataType = fetchdatafromScript.ParseColumnNameAndDataType(_columnDetails, false, ref errorMessage, ref isValidColumnFamily);


                _exportPocoList.Add(Parser.ConvertGeneratedMetaDataToListFormat(namespaceName, _columnFamilyName, _keyDataType, columnsAndDataType, isCqlScript));
            }
        }

        /// <summary>
        /// This function Generates List from the CLI Script Provided decided after it is checked in previuos Function.
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <param name="script"></param>
        /// <param name="errorMessage"></param>
        /// <param name="isCqlScript"></param>
        /// <param name="columnsAndDataType"></param>
        /// <returns>Generates List<POCOObjectListForExport/> Retained As Global Variable</returns>
        private static void GenerateListForScriptCli(string namespaceName, string script, ref string errorMessage, bool isCqlScript, ref Dictionary<string, string> columnsAndDataType)
        {
            var fetchdatafromScript = new FetchDataFromScript();
            const string createColumnFamilyCli = "create column family";
            const string keyValidationClass = "key_validation_class";
            const string withCompartor = "with comparator";
            const string columnMetaData = "and column_metadata";
            var isValidColumnFamily = Validator.ValidateCliScript(script,ref errorMessage, createColumnFamilyCli);
            if (!isValidColumnFamily) return;
            // Fetch Column family name
            _columnFamilyName = fetchdatafromScript.FetchData(script, createColumnFamilyCli, withCompartor);
            // Identify the key data type
            _keyDataType = fetchdatafromScript.FetchData(script, keyValidationClass, columnMetaData);
            // Identify the Column Details
            _columnDetails = fetchdatafromScript.FetchData(script, "[", "]");

            isValidColumnFamily = Validator.ValidateCliScriptsColumnDetails(ref errorMessage, _columnFamilyName, _keyDataType);

            if (!isValidColumnFamily) return;
            columnsAndDataType = fetchdatafromScript.ParseColumnNameAndDataType(_columnDetails, true, ref errorMessage, ref isValidColumnFamily);

            _exportPocoList.Add(Parser.ConvertGeneratedMetaDataToListFormat(namespaceName, _columnFamilyName, _keyDataType, columnsAndDataType, isCqlScript));
        }

        #endregion

      
        #region  Script generation from  POCO (CSharp) To DBScript(CQL)

        /// <summary>
        /// This function is to Generate List if any file has multiple classes for CSharp Class.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="errorMessage"></param>
        /// <param name="isValidScript"></param>
        /// <returns></returns>
        public static List<PocoObjectListForExport> GenerateListByExtractingMultipleClassesCSharp(string script, ref string errorMessage, ref bool isValidScript)
        {
            string[] tokens = { };
            const string delimiter = "public class ";
            if (script.Contains(delimiter))
                tokens = script.Split(new[] { delimiter }, StringSplitOptions.None);
            var scriptCollection = new List<string>(tokens);
            var startindex = scriptCollection[0].IndexOf("namespace", StringComparison.Ordinal);
            var length = scriptCollection[0].IndexOf("{", StringComparison.Ordinal) - startindex;
            var namespaceName = scriptCollection[0].Substring(startindex, length) + "{";
            if (namespaceName.Contains("."))
            {
                namespaceName = namespaceName.Remove(namespaceName.IndexOf('.'));
            }
            scriptCollection.RemoveAt(0);
            for (var i = 0; i < scriptCollection.Count; i++)
            {
                scriptCollection[i] = scriptCollection[i].Insert(0, "using system;   " + namespaceName + " " + delimiter);
            }
            foreach (var individualScript in scriptCollection)
            {
                _exportPocoList = GenerateListFromCSharpScript(individualScript.ToString(CultureInfo.InvariantCulture), out errorMessage, out isValidScript);
            }
            return _exportPocoList;
        }

        /// <summary>
        /// This function Generates Script for CSharp Poco Provided from Clipboard.
        /// </summary>
        ///<param name="pocoScript"></param>
        ///<param name="errorMessage"></param>
        ///<param name="isValidScript"></param>
        /// <returns>Returns Generated Script From List Provided</returns>
        public static string GenerateCqlScriptFromPocoCSharpScript(string pocoScript, ref string errorMessage, ref bool isValidScript)
        {
            return LangParser.CreateCqlScriptFromList(GenerateListByExtractingMultipleClassesCSharp(pocoScript, ref errorMessage, ref isValidScript));
        }

        /// <summary>
        /// This function Generates Script for CSharp Poco Provided From Current Tab In Browse Functionality.
        /// </summary>
        ///<param name="pocoScript"></param>
        ///<param name="errorMessage"></param>
        ///<param name="isValidScript"></param>
        /// <returns>Returns Generated Script From List Provided</returns>
        private static List<PocoObjectListForExport> GenerateListFromCSharpScript(string pocoScript, out string errorMessage, out bool isValidScript)
        {
            errorMessage = string.Empty;
            const string startOfScript = "using system";
            isValidScript= LangValidator.ValidateScriptsOfSelectedLanguageType(pocoScript,ref errorMessage, startOfScript);

            if (isValidScript)
            {
                var fetchdatafromScript = new FetchDataFromScript();
                //Fetch KeyspaceName(Table) name
                _keyspaceName = fetchdatafromScript.FetchData(pocoScript, "namespace", "{");
                // Fetch Column family(Table) name
                _columnFamilyName = fetchdatafromScript.FetchData(pocoScript, "public class", "{");
                _columnDetails = fetchdatafromScript.FetchData(pocoScript, "{", "}");

                isValidScript = LangValidator.ValidateColumnDetailsForSelectedLanguageType(_keyspaceName, _columnFamilyName, ref errorMessage, _columnDetails);

                if (isValidScript)
                {
                    var columnsAndDataType = fetchdatafromScript.ParseColumnNameAndDataTypeForCSharpScript(_columnDetails, ref errorMessage, ref isValidScript);
                    var pocoChildList = Parser.PocoObjectListMetadata(columnsAndDataType);
                    _exportPocoList.Add(LangParser.ConvertFetchedDataToExportFormatInPocoToDb(_keyspaceName, _columnFamilyName, pocoChildList));
                    
                }
            }
            return _exportPocoList;


        }

        #endregion

        #region  Script generation from  POCO  (VB) To DBScript(CQL)

        /// <summary>
        /// This function is to Generate List if any file has multiple classes for VB Class.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="errorMessage"></param>
        /// <param name="isValidScript"></param>
        /// <returns></returns>
        public static List<PocoObjectListForExport> GenerateListByExtractingMultipleClassesVb(string script, ref string errorMessage, ref bool isValidScript)
        {
            string[] tokens = { };
            const string delimiter = "public class ";
            if (script.Contains(delimiter))
                tokens = script.Split(new[] { delimiter }, StringSplitOptions.None);
            var scriptCollection = new List<string>(tokens);
            var startindex = scriptCollection[0].IndexOf("namespace", StringComparison.Ordinal);
            var length = scriptCollection[0].Length - startindex;
            var namespaceName = scriptCollection[0].Substring(startindex, length);
            scriptCollection.RemoveAt(0);
            for (var i = 0; i < scriptCollection.Count; i++)
            {
                scriptCollection[i] = scriptCollection[i].Insert(0, "imports system   " + namespaceName + " " + delimiter);
            }
            foreach (var individualScript in scriptCollection)
            {
                _exportPocoList = GenerateListFromVbScript(individualScript.ToString(CultureInfo.InvariantCulture), ref errorMessage, out isValidScript);
            }
            return _exportPocoList;
        }

        /// <summary>
        /// This function creates CQL Script from VB Script Provided.
        /// </summary>
        /// <param name="pocoScript"></param>
        /// <param name="isValidScript"></param>
        /// <param name="errorMessage"></param>
        /// <returns>Returns CQL Script</returns>
        public static string GenerateCqlScriptFromPocoVbScript(string pocoScript, ref string errorMessage, ref bool isValidScript)
        {
            return LangParser.CreateCqlScriptFromList(GenerateListByExtractingMultipleClassesVb(pocoScript, ref errorMessage, ref isValidScript));

        }

        /// <summary>
        /// This function creates List from VB Script Provided.
        /// </summary>
        /// <param name="pocoScript"></param>
        /// <param name="isValidScript"></param>
        /// <param name="errorMessage"></param>
        /// <returns>Returns List</returns>
        private static List<PocoObjectListForExport> GenerateListFromVbScript(string pocoScript, ref string errorMessage, out bool isValidScript)
        {
            const string startOfScript = "imports system";
            isValidScript = LangValidator.ValidateScriptsOfSelectedLanguageType(pocoScript, ref errorMessage, startOfScript);

            if (isValidScript)
            {
                var fetchdatafromScript = new FetchDataFromScript();
                //Fetch KeyspaceName(Table) name
                _keyspaceName = fetchdatafromScript.FetchData(pocoScript, "namespace", "public");
                // Fetch Column family(Table) name
                _columnFamilyName = fetchdatafromScript.FetchData(pocoScript, "public class", "public");
                _columnDetails = fetchdatafromScript.FetchData(pocoScript, _columnFamilyName, "end class");

                isValidScript = LangValidator.ValidateColumnDetailsForSelectedLanguageType(_keyspaceName, _columnFamilyName, ref errorMessage, _columnDetails);

                if (isValidScript)
                {
                    var columnsAndDataType = fetchdatafromScript.ParseColumnNameAndDataTypeForVbScript(_columnDetails, ref errorMessage, ref isValidScript);
                    var pocoChildList = Parser.PocoObjectListMetadata(columnsAndDataType);
                    _exportPocoList.Add(LangParser.ConvertFetchedDataToExportFormatInPocoToDb(_keyspaceName, _columnFamilyName, pocoChildList));

                }
            }

            return _exportPocoList;
        }

        #endregion


        #region Language Type

        /// <summary>
        /// This function fetch the language Type Assigned By Script Type
        /// </summary>
        /// <param name="isVb"></param>
        /// <param name="isJava"></param>
        /// <param name="isCSharp"></param>
        /// <param name="isRuby"></param>
        /// <returns>Returns Script Type as Reference Variables</returns>
     
        public static void GetLanguageType(out bool isVb, out bool isJava, out bool isCSharp, out bool isRuby)
        {
            LanguageSelector.GetLanguageType(out  isVb, out  isJava, out  isCSharp, out  isRuby);
        }

        #endregion

        #region Generate Script From Metadata Of DB

        /// <summary>
        /// This function generates list from the columnfamily selected from treeview.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns>Returns List</returns>
  
        public static List<PocoObjectListForExport> GenerateListForCreateColumnFamilyMetdata(string parent, ColumnFamilyName child)
        {
            _exportPocoList.AddRange(Reader.GenerateListForColumnFamilyMetdata(parent, child));
            return _exportPocoList;
        }
        #endregion

        #region Reinitialize Or Fetch ExportList

        /// <summary>
        /// This function reinitializes the List Retained at Engine Level.
        /// </summary>
        public static void ReinitializeExportList()
        {
            _exportPocoList = new List<PocoObjectListForExport>();
        }

        /// <summary>
        /// This function returns the retained List to UI
        /// </summary>
        /// <returns>Returns List To UI</returns>
    
        public static List<PocoObjectListForExport> FetchExportList()
        {
            return _exportPocoList;
        } 

        #endregion

        #region DBScript Writer

        /// <summary>
        /// This function creates CQL Script from List Created from POCO Class
        /// </summary>
        /// <param name="pocoList"></param>
        /// <returns>Returns CQL Script</returns>
        public static string CreateCqlScriptFromList(List<PocoObjectListForExport> pocoList)
        {
            _exportPocoList = pocoList;
            return LangParser.CreateCqlScriptFromList(pocoList);
        } 

        #endregion

       

    }
}
