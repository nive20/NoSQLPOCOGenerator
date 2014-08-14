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
    public  class CSharpLangWriter
    {

        #region Writer Class In Form of c# Script

        /// <summary>
        /// This function is the Writer Implementation of Generating Csharp Script
        /// </summary>
        /// <param name="exportPocoList"></param>
        /// <param name="exportItem"></param>
        /// <param name="namespaceName"></param>
        /// <returns>Return CSharp Script</returns>
        public  string Writer(List<PocoObjectListForExport> exportPocoList, PocoObjectListForExport exportItem, string namespaceName)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("using System");
            stringBuilder.AppendLine("namespace " + namespaceName);
            stringBuilder.AppendLine("{");
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

            stringBuilder.AppendLine("}");
            var pocoScript = stringBuilder.ToString();
            return pocoScript;
        }

        #endregion

        #region Script Generation Per Object

        /// <summary>
        /// This function generates Script Per Object.
        /// </summary>
        /// <param name="parentItem"></param>
        /// <returns>Returns Class Data As String</returns>
        private static string SaveAsFileOptionPerObject(PocoObjectListForExport parentItem)
        {
            var stringBuilder = new StringBuilder();
            const string getsetProp = "{ get;  set; } ";
            stringBuilder.AppendLine("public class " + parentItem.ClassName);
            stringBuilder.AppendLine("{");
            foreach (var childItem in parentItem.PocoListObjects)
            {
                stringBuilder.AppendFormat("public " + childItem.Type + "    " + childItem.ColumnFamilyName);
                stringBuilder.Append("  " + getsetProp + "\n");

            }
            stringBuilder.AppendLine("}");
            return stringBuilder.ToString();
        }

        #endregion

    }
}
