/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

namespace POCOGenerator.LanguageProvider
{
    public static class LanguageSelector
    {

        #region Global Declarations

        private static bool _isJava;
        private static bool _isVb;
        private static bool _isCSharp;
        private static bool _isRuby;
        #endregion

        #region Assign Language Type for both POCO To DB and vice-versa.

        /// <summary>
        /// This function Assign Selected Language - DB To POCO
        /// </summary>
        /// <param name="selectedLanguage"></param>
        /// <returns>Returns bool</returns>
        public static bool AssignSelectedLanguageTypeDbToPoco(string selectedLanguage)
        {
            _isVb = false;
            _isJava = false;
            _isCSharp = false;
            _isRuby = false;

            if (selectedLanguage == "VB")
                _isVb = true;
            else if (selectedLanguage == "CSharp")
                _isCSharp = true;
            else if (selectedLanguage == "Java")
                _isJava = true;
            else if (selectedLanguage == "Ruby")
                _isRuby = true;
            else
                return false;
            return true;
        }

        /// <summary>
        /// This function Assign Selected Language - POCO To DB
        /// </summary>
        /// <param name="script"></param>
        /// <returns>Returns bool</returns>
        public static bool AssignSelectedLanguageTypePocoToDb(string script)
        {
            script = script.ToLower();
            _isVb = false;
            _isJava = false;
            _isCSharp = false;
            _isRuby = false;
            var isValidScript = LangValidator.ValidateScriptAsPocoClass(script);
            if (script.StartsWith("imports"))
                isValidScript =_isVb = true;
            else if (script.StartsWith("using"))
               isValidScript= _isCSharp = true;
            else return isValidScript;
            return isValidScript;
        }

        #endregion

        #region Get Language type

        /// <summary>
        /// This function gets the Language Type Assigned.
        /// </summary>
        /// <param name="vb"></param>
        /// <param name="java"></param>
        /// <param name="cSharp"></param>
        /// <param name="ruby"></param>
        /// 
        public static void GetLanguageType(out bool vb, out bool java, out bool cSharp, out bool ruby)
        {
            vb = _isVb;
            java = _isJava;
            cSharp = _isCSharp;
            ruby = _isRuby;
        }

        #endregion



    }
}
