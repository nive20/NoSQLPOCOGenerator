/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System.Collections.Generic;
using POCOGenerator.DatabaseObjects;
using POCOGenerator.LanguageProvider;

namespace POCOGenerator.Engine
{
    public class ScriptWriter:ILanguageProvider
    {
        readonly WriterClass _writer = new WriterClass();

        #region Fetch Language Type

        /// <summary>
        /// This function returns the Language Type Assigned.
        /// </summary>
        public  void GetLanguageType(out bool isVb, out bool isJava, out bool isCSharp, out bool isRuby)
        {
            LanguageSelector.GetLanguageType(out  isVb, out  isJava, out  isCSharp, out  isRuby);
        }

        #endregion

        #region Call to Writer Implementation

      

        /// <summary>
        /// This function writes Scripts As Per Selected Script Type
        /// </summary>
        /// <param name="exportPocoList"></param>
        /// <param name="exportItem"></param>
        /// <param name="fileName"></param>
        /// <param name="namespaceName"></param>
        /// <returns>Returns the Writer Work is Finished and Saved as bool<POCOObjectListForExport/></returns>
      
        public bool WriteScriptAsPerSelectedLanguage(List<PocoObjectListForExport> exportPocoList, PocoObjectListForExport exportItem, string namespaceName, string fileName)
        {
            return _writer.GenerateScriptAsPerSelectedLanguage(exportPocoList, exportItem, namespaceName, fileName);
        } 

        #endregion

    }
}
