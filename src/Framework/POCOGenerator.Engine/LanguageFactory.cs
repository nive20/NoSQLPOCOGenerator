/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  


 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using POCOGenerator.LanguageProvider;

namespace POCOGenerator.Engine
{
    public static class LanguageFactory
    {  

        #region Load Language Writer As per Script Type Selected

        /// <summary>
        /// This function the Loads the LanguageWriter specific to  ScriptType provided.
        /// </summary>
        /// <param name="scriptType"></param>
        /// <returns>Returns Language Provider for specific Language Exists or not.</returns>
        public static bool LoadLanguageWriter(ScriptType scriptType)
        {
            var selectedLanguage = string.Empty;

            switch (scriptType)
            {
                case ScriptType.CSharp:
                    selectedLanguage = "CSharp";
                    break;
                case ScriptType.Vb:
                    selectedLanguage = "VB";
                    break;
                case ScriptType.Java:
                    selectedLanguage = "Java";
                    break;
                case ScriptType.Ruby:
                    selectedLanguage = "Ruby";
                    break;
            }

            var isExists = LanguageSelector.AssignSelectedLanguageTypeDbToPoco(selectedLanguage);
            return isExists;
        }

        /// <summary>
        /// This function Loads Langauge Reader as whichever Script Type is Provided.
        /// </summary>
        /// <param name="script"></param>
        /// <returns>Returns Language Reader for Specified ScriptType as bool</returns>
        public static bool LoadLanguageReader(string script)
        {
            var isExists = LanguageSelector.AssignSelectedLanguageTypePocoToDb(script);
            return isExists;
        }

        #endregion
    }
}
