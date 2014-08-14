/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  


 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System.Collections.Generic;
using POCOGenerator.DatabaseObjects;

namespace POCOGenerator.Engine
{
    public interface ILanguageProvider
    {
        #region Fetch language Selected

        /// <summary>
        /// This Function gets the language type.
        /// </summary>
        /// <param name="isVb"></param>
        /// <param name="isJava"></param>
        /// <param name="isCSharp"></param>
        /// <param name="isRuby"></param>
        void GetLanguageType(out bool isVb, out bool isJava, out bool isCSharp, out bool isRuby); 
        #endregion

        #region Writer As per Selected Language
        /// <summary>
        /// This function writes Script as per language Provided.
        /// </summary>
        /// <param name="exportPocoList"></param>
        /// <param name="exportItem"></param>
        /// <param name="namespaceName"></param>
        /// <param name="fileName"></param>
        /// <returns>Returns Script is Written and Saved as bool</returns>
        bool WriteScriptAsPerSelectedLanguage(List<PocoObjectListForExport> exportPocoList, PocoObjectListForExport exportItem, string namespaceName, string fileName);
        
        #endregion
    }
}
