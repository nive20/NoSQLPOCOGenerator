/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System.Collections.Generic;
using System.Text;
using POCOGenerator.DatabaseObjects;


namespace POCOGenerator.LanguageProvider
{
    public  class VbLangWriter
    {

        #region Writer Class In Form of VB Script

        /// <summary>
        /// This function implements writer functionality  VB Script generation.
        /// </summary>
        /// <param name="exportPocoList"></param>
        /// <param name="exportItem"></param>
        /// <param name="namespaceName"></param>
        /// <returns>Return VB Script</returns>
        public  string Writer(List<PocoObjectListForExport> exportPocoList, PocoObjectListForExport exportItem, string namespaceName)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Imports System");
            stringBuilder.AppendLine("Namespace " + namespaceName);
            stringBuilder.AppendLine("");
            if (exportItem == null)
            {
                foreach (var parentItem in exportPocoList)
                {
                    stringBuilder.AppendLine(SaveAsFileOptionPerObject(parentItem));
                    stringBuilder.AppendLine();
                }
            }
            else
                stringBuilder.AppendLine(SaveAsFileOptionPerObject(exportItem));
            stringBuilder.AppendLine("End Namespace");
            var pocoScript = stringBuilder.ToString();
            return pocoScript;
        }

        #endregion

        #region Script Generation Per Object

        /// <summary>
        /// Generates Writer functionality Per Object
        /// </summary>
        /// <param name="parentItem"></param>
        /// <returns>Returns VB Script</returns>
        private static string SaveAsFileOptionPerObject(PocoObjectListForExport parentItem)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Public Class " + parentItem.ClassName);
            stringBuilder.AppendLine();
            foreach(var childItem in parentItem.PocoListObjects)
            {
                stringBuilder.AppendFormat("Public Property " + childItem.Type + "  As  " + childItem.ColumnFamilyName);
                stringBuilder.AppendLine();
            }
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("End Class");
            return stringBuilder.ToString();
        }

        #endregion
    }
}
