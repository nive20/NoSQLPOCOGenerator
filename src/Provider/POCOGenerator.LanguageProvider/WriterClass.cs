/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System.Collections.Generic;
using System.IO;
using POCOGenerator.DatabaseObjects;

namespace POCOGenerator.LanguageProvider
{
    public  class WriterClass
    {

        # region Global Variables Section

        private static bool _isJava;
        private static bool _isVb;
        private static bool _isCSharp;
        private static bool _isRuby;
        private static bool _isSaved;
        private static string _pocoScript = string.Empty;
        readonly CSharpLangWriter _csharpWriter = new CSharpLangWriter();
        readonly VbLangWriter _vbWriter = new VbLangWriter();

        #endregion

        #region Generate Script As Per Selected Language Type

        /// <summary>
        /// This function generates Script as per Selected Language and Writes to File
        /// </summary>
        /// <param name="conversionList"></param>
        /// <param name="exportItem"></param>
        /// <param name="namespaceName"></param>
        /// <param name="fileName"></param>
        /// <returns>Returns bool</returns>
        public bool GenerateScriptAsPerSelectedLanguage(List<PocoObjectListForExport> conversionList, PocoObjectListForExport exportItem, string namespaceName, string fileName)
        {
            LanguageSelector.GetLanguageType(out _isVb, out _isJava, out _isCSharp, out _isRuby);

            try
            {
                using (var sw = new StreamWriter(fileName))
                {

                    _pocoScript = _isVb ? _vbWriter.Writer(conversionList, exportItem, namespaceName) : 
                        _csharpWriter.Writer(conversionList, exportItem, namespaceName);

                    sw.WriteLine(_pocoScript);

                    _isSaved = true;
                }
            }
            catch (IOException )
            {

                _isSaved = false;
                return _isSaved;
            }
            return _isSaved;
        }

        #endregion

    }
}
