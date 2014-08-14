/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System.Text.RegularExpressions;

namespace POCOGenerator.LanguageProvider
{  
    public static class LangValidator
    {
        static bool _isValidScript = true;

        #region Poco To DB Script Validation

        /// <summary>
        /// This function Validates columns and DataType
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="columnName"></param>
        /// <param name="dataType"></param>
        /// <returns>Returns bool</returns>
        public static bool ValidateColumnsAndDataType(ref string errorMessage, string columnName, string dataType)
        {
            bool isValidColumnFamily = true;
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(dataType))
            {
                errorMessage = "Column names / DataType is not defined properly. Please verify the script.";
                isValidColumnFamily = false;
            }

            return isValidColumnFamily;
        }

        /// <summary>
        /// This function Validates Scripts According to its Language Type Selected.
        /// </summary>
        /// <param name="pocoScript"></param>
        /// <param name="errorMessage"></param>
        /// <param name="startOfScript"></param>
        /// <returns>Returns bool</returns>
        public static bool ValidateScriptsOfSelectedLanguageType(string pocoScript,ref string errorMessage, string startOfScript)
        {
            _isValidScript = ScriptValidationByLanguageType(pocoScript, ref errorMessage);

            if (string.IsNullOrEmpty(pocoScript))
            {
                errorMessage = "Please provide the POCO Class Script on the left panel window";
                _isValidScript = false;
            }

            if (pocoScript == null || pocoScript.StartsWith(startOfScript)) return _isValidScript;
            errorMessage = "POCO Class Script is in Incorrect format";
            _isValidScript = false;
            return _isValidScript;
        }

        /// <summary>
        /// This functions contains the logic for validating language for type selected.
        /// </summary>
        /// <param name="pocoScript"></param>
        /// <param name="errorMessage"></param>
        /// <returns>Returns bool</returns>
        private static bool ScriptValidationByLanguageType(string pocoScript, ref string errorMessage)
        {
            bool vb;
            bool java;
            bool cSharp;
            bool ruby;
            LanguageSelector.GetLanguageType(out vb, out java, out cSharp, out ruby);
            if (vb)
            {
                if ( !pocoScript.Contains("public class")|| !pocoScript.Contains("end class"))
                {
                    errorMessage = "Please provide the complete Script";
                    _isValidScript = false; 
                }
            }

            if (!cSharp) return _isValidScript;

            var startcount = Regex.Matches(pocoScript, "{").Count;
            var endCount = Regex.Matches(pocoScript, "}").Count;
            if (startcount != endCount || !pocoScript.Contains("{") || !pocoScript.Contains("}"))
            {
                errorMessage = "Please verify the Script";
                _isValidScript = false;
            }
            return _isValidScript;
        }

        /// <summary>
        /// This functions Validates the ColumnDetails for Script of selected language type.
        /// </summary>
        /// <param name="keyspaceName"></param>
        /// <param name="columnFamilyName"></param>
        /// <param name="errorMessage"></param>
        /// <param name="objectDetails"></param>
        /// <returns>Returns bool</returns>
        public static bool ValidateColumnDetailsForSelectedLanguageType(string keyspaceName, string columnFamilyName, ref string errorMessage, string objectDetails)
        {
            if (string.IsNullOrEmpty(keyspaceName))
            {
                errorMessage = "Not able to identify the column family name.  Please verify the POCO Class.";
                _isValidScript = false;
            }

            if (string.IsNullOrEmpty(columnFamilyName))
            {
                errorMessage = "Not able to identify the column family name.  Please verify the POCO Class.";
                _isValidScript = false;
            }

            if (!string.IsNullOrEmpty(objectDetails)) return _isValidScript;
            errorMessage = "Column details are missing.  Please verify the script.";
            _isValidScript = false;

            return _isValidScript;
        }

        #endregion


        public static bool ValidateScriptAsPocoClass(string script)
        {
            if ((script.Contains("get") && script.Contains("set") ) || script.Contains("property"))
                return true;
            return false;
        }
    }
}
